using AutoMapper;
using LoanMe.Catalog.Api.Application.Entities;
using LoanMe.Catalog.Api.Application.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
			services.AddDbContext<DataContext>(
				(ops) => ops.UseMySql(Configuration.GetConnectionString("Customers")));

			services.AddScoped<DataContext>();
			services.AddScoped<ICustomerRepository, CustomerRepository>();
			
			// Add Swagger
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1",
					new Info
					{
						Version = "v1.0.0",
						Title = "LoanMe Customers API",
						Description = "API to expose LoanMe Custumer logic",
						TermsOfService = ""
					}
				);
			});

			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy",
					builder => builder.AllowAnyOrigin()
					.WithMethods(
						"GET",
						"POST",
						"PUT",
						"DELETE",
						"OPTIONS")
					.AllowAnyHeader()
					.AllowCredentials());
			});

			services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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
}
