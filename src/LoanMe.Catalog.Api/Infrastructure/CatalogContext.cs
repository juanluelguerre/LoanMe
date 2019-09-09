using LoanMe.Catalog.Api.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LoanMe.Catalog.Api.Application.Entities
{
	public class CatalogContext : DbContext
	{
		public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
		{			
		}

		public DbSet<CatalogItem> CatalogItems { get; set; }
		public DbSet<CatalogBrand> CatalogBrands { get; set; }
		public DbSet<CatalogType> CatalogTypes { get; set; }


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			//builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
			//builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
			//builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());

			builder.ApplyConfigurationsFromAssembly(typeof(Startup).Assembly);
		}

		public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<CatalogContext>
		{
			public CatalogContext CreateDbContext(string[] args)
			{
				IConfigurationRoot configuration = new ConfigurationBuilder()
				   .SetBasePath(Directory.GetCurrentDirectory())
				   .AddUserSecrets<Startup>()
				   .AddJsonFile("appsettings.json")
				   .Build();

				var builder = new DbContextOptionsBuilder<CatalogContext>();

				builder.UseSqlServer(configuration.GetConnectionString("DataBaseConnection"));

				return new CatalogContext(builder.Options);
			}
		}
	}
}
