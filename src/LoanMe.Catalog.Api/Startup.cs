using AutoMapper;
using LoanMe.Catalog.Api.Application.Entities;
using LoanMe.Catalog.Api.Infrastructure.Filters;
using LoanMe.Catalog.Api.IntegrationEvents.EventHandling;
using LoanMe.EventBus.IntegrationEventLogEF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
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

	internal static class CustomExtensionMethods
	{
		private const string DATABASE_CONNECIONSTRING = "DataBaseConnection";

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
				options.UseSqlServer(configuration.GetConnectionString(DATABASE_CONNECIONSTRING),
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
				options.UseSqlServer(configuration.GetConnectionString(DATABASE_CONNECIONSTRING),
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
			services.AddTransient<OrderStatusChangeToAwaitingValidationEventHandler>();
			services.AddTransient<OrderStatusChangedToFinanceEventHandler>();
			services.AddTransient<ProductPriceChangeEventHandler>(); 

			return services;
		}

		public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddCap(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString(DATABASE_CONNECIONSTRING));

				if (configuration.GetValue<bool>(nameof(CatalogSettings.AzureServiceBusEnabled)))
				{
					options.UseAzureServiceBus(configuration.GetConnectionString(nameof(CatalogSettings.EventBusConnection)));
				}
				else
				{
					options.UseRabbitMQ(conf =>
					{
						conf.HostName = configuration.GetConnectionString(nameof(CatalogSettings.EventBusConnection));
						if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
						{
							conf.UserName = configuration["EventBusUserName"];
						}
						if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
						{
							conf.Password = configuration["EventBusPassword"];
						}
					});
				}

				if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
				{
					options.FailedRetryCount = int.Parse(configuration["EventBusRetryCount"]);
				}

				if (!string.IsNullOrEmpty(configuration["SubscriptionClientName"]))
				{
					options.DefaultGroup = configuration["SubscriptionClientName"];
				}
			});

			return services;
		}
	}
}
