using AutoMapper;
using LoanMe.Catalog.Api.Infrastructure.Filters;
using LoanMe.Catalog.Api.Application.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using Swashbuckle.AspNetCore.Swagger;

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

			//services.AddDbContext<DataContext>(
			//	  opt => opt.UseInMemoryDatabase("Customers")
			//		.ConfigureWarnings(cw => cw.Ignore(InMemoryEventId.TransactionIgnoredWarning)));			

			// EF Connnection
			services.AddDbContext<CatalogContext>(
				(ops) => ops.UseMySql(Configuration.GetConnectionString("Catalog")));

			services.AddScoped<CatalogContext>();

			services				
				.AddEventBus(Configuration)
				.AddCustomMVC(Configuration)
				.AddSwagger();

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

		public static IServiceCollection AddCustomMVC(this IServiceCollection services, IConfiguration configuration)
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

		public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton((_) =>
			{
				var endpointConfiguration = new EndpointConfiguration("Catalog");

				endpointConfiguration.UseTransport<LearningTransport>();

				endpointConfiguration.UseContainer<ServicesBuilder>(
					customizations: customizations =>
					{
						customizations.ExistingServices(services);
					});

				var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
				return endpoint;
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
	}
}
