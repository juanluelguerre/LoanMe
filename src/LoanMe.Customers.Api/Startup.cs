using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LoanMe.Customers.Api.Application.Domain.Aggregates;
using LoanMe.Customers.Api.Application.Domain.Interfaces;
using LoanMe.Customers.Api.Application.Data;
using Swashbuckle.AspNetCore.Swagger;

namespace LoanMe.Customers.Api
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
			//	  opt => opt.UseInMemoryDatabase("MyBudget")
			//		.ConfigureWarnings(cw => cw.Ignore(InMemoryEventId.TransactionIgnoredWarning)));			

			// EF Connnection
			services.AddDbContext<DataContext>(
				(ops) => ops.UseMySQL(Configuration.GetConnectionString("Customers")));

			services.AddScoped<DataContext>();
			services.AddScoped<IDataService<Customer>, DataRepository<Customer>>();
			services.AddScoped<IDataService<CustomerAccount>, DataRepository<CustomerAccount>>();
			// services.AddScoped<IDataService<Budget>, DataRepository<Budget>>();			

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

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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

			app.UseSwagger()
				.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyBudget v1.0.0");
				});


			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseMvc();
		}		
	}
}
