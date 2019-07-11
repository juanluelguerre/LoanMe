using AutoMapper;
using Catalog.API.IntegrationEvents;
using LoanMe.ApplicationBlocks.EventBus;
using LoanMe.ApplicationBlocks.EventBus.Abstractions;
using LoanMe.ApplicationBlocks.EventBusRabbitMQ;
using LoanMe.Catalog.Api.Application.Entities;
using LoanMe.Catalog.Api.Infrastructure.Filters;
using LoanMe.Catalog.Api.Services;
using LoanMe.Catalog.Api.Services.EventHandling;
using LoanMe.EventBus.EventBusServiceBus;
using LoanMe.EventBus.IntegrationEventLogEF;
using LoanMe.EventBus.IntegrationEventLogEF.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Data.Common;
using System.Reflection;

namespace LoanMe.Catalog.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
			//	.AddAzureADBearer(options => Configuration.Bind("AzureAd", options));			

			services
				//.AddAppInsight(Configuration)
				.AddCustomMVC()
				.AddCustomDbContext(Configuration)
				.AddCustomOptions(Configuration)
				.AddIntegrationServices(Configuration)
				.AddEventBus(Configuration)
				.AddSwagger();
				// .AddCustomHealthCheck(Configuration);

			services.AddAutoMapper(typeof(Startup));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseCors("CorsPolicy");

			app.UseSwagger()
				.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "LoanMe v1.0.0");
				});


			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseMvc();
		}
	}

	public static class CustomExtensionMethods
	{
		//public static IServiceCollection AddAppInsight(this IServiceCollection services, IConfiguration configuration)
		//{
		//}

		public static IServiceCollection AddCustomMVC(this IServiceCollection services)
		{
			services.AddMvc(options =>
			{
				options.Filters.Add(typeof(HttpGlobalExceptionFilter));
			})
			.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
			.AddControllersAsServices();

			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy",
					builder => builder
					// builder => builder.AllowAnyOrigin()
					.SetIsOriginAllowed((host) => true)
					.WithMethods(
						"GET",
						"POST",
						"PUT",
						"DELETE",
						"OPTIONS")
					.AllowAnyHeader()
					.AllowCredentials());
			});

			return services;
		}

		public static IServiceCollection AddSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(options =>
			{
				options.DescribeAllEnumsAsStrings();
				options.SwaggerDoc("v1", new Info
				{
					Version = "v1.0.0",
					Title = "LoanMe Customers API",
					Description = "API to expose LoanMe Custumer logic",
					TermsOfService = ""
				});
			});

			return services;
		}

		public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<CatalogContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("Catalog"),
									 sqlServerOptionsAction: sqlOptions =>
									 {
										 sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
										 //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
										 sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
									 });

				// Changing default behavior when client evaluation occurs to throw. 
				// Default in EF Core would be to log a warning when client evaluation is performed.
				options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
				//Check Client vs. Server evaluation: https://docs.microsoft.com/en-us/ef/core/querying/client-eval
			});

			services.AddDbContext<IntegrationEventLogContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("Catalog"),
									 sqlServerOptionsAction: sqlOptions =>
									 {
										 sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
										 //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
										 sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
									 });
			});

			return services;
		}

		public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<CatalogSettings>(configuration);
			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = context =>
				{
					var problemDetails = new ValidationProblemDetails(context.ModelState)
					{
						Instance = context.HttpContext.Request.Path,
						Status = StatusCodes.Status400BadRequest,
						Detail = "Please refer to the errors property for additional details."
					};

					return new BadRequestObjectResult(problemDetails)
					{
						ContentTypes = { "application/problem+json", "application/problem+xml" }
					};
				};
			});

			return services;
		}

		public static IServiceCollection AddIntegrationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
				sp => (DbConnection c) => new IntegrationEventLogService(c));

			services.AddTransient<ICatalogEventService, CatalogEventService>();

			if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
			{
				services.AddSingleton<IServiceBusPersisterConnection>(sp =>
				{
					var settings = sp.GetRequiredService<IOptions<CatalogSettings>>().Value;
					var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();

					var serviceBusConnection = new ServiceBusConnectionStringBuilder(settings.EventBusConnection);

					return new DefaultServiceBusPersisterConnection(serviceBusConnection, logger);
				});
			}
			else
			{
				services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
				{
					var settings = sp.GetRequiredService<IOptions<CatalogSettings>>().Value;
					var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

					var factory = new ConnectionFactory()
					{
						HostName = configuration["EventBusConnection"],
						DispatchConsumersAsync = true
					};

					if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
					{
						factory.UserName = configuration["EventBusUserName"];
					}

					if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
					{
						factory.Password = configuration["EventBusPassword"];
					}

					var retryCount = 5;
					if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
					{
						retryCount = int.Parse(configuration["EventBusRetryCount"]);
					}

					return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
				});
			}

			return services;
		}

		public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
		{
			var subscriptionClientName = configuration["SubscriptionClientName"];

			if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
			{
				services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
				{
					var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();					
					var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
					var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

					return new EventBusServiceBus(serviceBusPersisterConnection, logger,
						eventBusSubcriptionsManager, sp, subscriptionClientName);
				});

			}
			else
			{
				services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
				{
					var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();					
					var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
					var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

					var retryCount = 5;
					if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
					{
						retryCount = int.Parse(configuration["EventBusRetryCount"]);
					}

					return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, 
						eventBusSubcriptionsManager, sp, subscriptionClientName, retryCount);
				});
			}

			services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
			services.AddTransient<CatalogPriceChangeEventHandler>();

			// TODO: Add handlser as needed !

			return services;
		}
	}
}
