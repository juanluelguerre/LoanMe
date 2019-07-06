using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LoanMe.Catalog.Api.Application.Entities
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}

		public DbSet<Customer> Customers { get; set; }
		public DbSet<CustomerAccount> CustomersAccount { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//	modelBuilder.Entity<Customer>().Property(a => a.BankAccounts).HasColumnName("BankAccount");
			//	modelBuilder.Entity<Customer>().HasKey(c => c.i

			//modelBuilder.Entity<CustomerAccount>().HasKey(c => new { c.Id, c.BankAccount });
			//modelBuilder.Entity<Customer>().HasMany(c => c.BankAccounts);

			//modelBuilder.Entity<Customer>().Property(c => c.Active).HasColumnType("tinyint(1)");
			//modelBuilder.Entity<CustomerAccount>().Property(ca => ca.MarkAsDefault).HasColumnType("tinyint(1)");
			// modelBuilder.Entity<CustomerAccount>().Property(ca => ca.BankAccount).HasColumnType("varchar(10)");

			//modelBuilder.Entity<CustomerAccount>().HasKey(c => new { c.Id, c.BankAccount });
			//modelBuilder.Entity<Customer>().HasMany(c => c.BankAccounts);

			var p1 = new Customer { Id = 1, FirstName = "Juan Luis", LastName = "Guerrero Minero", Active = true };
			var p2 = new Customer { Id = 2, FirstName = "Francisco", LastName = "Ruiz Vázquez", Active = true };
			var p3 = new Customer { Id = 3, FirstName = "Eva", LastName = "Perez Moreno", Active = true };
			var p4 = new Customer { Id = 4, FirstName = "Maria", LastName = "Serrano Sanchez", Active = true };

			var a1 = new CustomerAccount { CustomerId = 1, BankAccount = "ES12-1234-1234-1234567890", MarkAsDefault = true };
			var a2 = new CustomerAccount { CustomerId = 2, BankAccount = "ES13-4321-4321-0987654321", MarkAsDefault = true };

			modelBuilder.Entity<CustomerAccount>().HasData(
				a1, a2
			);

			modelBuilder.Entity<Customer>().HasData(
				p1, p2, p3, p4
			);
		}
	}
}
