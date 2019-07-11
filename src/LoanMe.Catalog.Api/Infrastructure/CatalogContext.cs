using LoanMe.Catalog.Api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

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
				var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>()
					.UseSqlServer("xxxxx");

				return new CatalogContext(optionsBuilder.Options);
			}
		}
	}
}
