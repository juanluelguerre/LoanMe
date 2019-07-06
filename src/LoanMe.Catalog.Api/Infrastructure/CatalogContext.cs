using LoanMe.Catalog.Api.Model;
using Microsoft.EntityFrameworkCore;

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
	}
}
