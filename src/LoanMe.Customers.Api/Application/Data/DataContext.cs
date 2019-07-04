using Microsoft.EntityFrameworkCore;
using LoanMe.Customers.Api.Application.Domain.Aggregates;
using System;

namespace LoanMe.Customers.Api.Application.Data
{
	public class DataContext : DbContext
	{
		// public const string TABLE_PERSON = "People";
		public const string TABLE_CUSTOMER = "Customers";
		public const string TABLE_CUSTOMER_ACCOUNT = "CustomersAccounts";
		// public const string TABLE_BUDGET = "Budgets";

		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// modelBuilder.Entity<Person>().ToTable(TABLE_PERSON);
			modelBuilder.Entity<Customer>().ToTable(TABLE_CUSTOMER);
			modelBuilder.Entity<CustomerAccount>().ToTable(TABLE_CUSTOMER_ACCOUNT);
			// modelBuilder.Entity<Budget>().ToTable(TABLE_BUDGET);

			// Bug: MySQL buhttps://stackoverflow.com/questions/47330796/entity-framework-core-with-mysql-unknown-column-in-field-list
			//		Also not suportend ICollection<> properties using "Pomelo..." Nuget.
			// modelBuilder.Entity<Customer>().Property(a => a.BankAccounts).HasColumnName("BankAccount");

			// https://github.com/aspnet/EntityFrameworkCore/issues/12278
			modelBuilder.Entity<Customer>().Property(c => c.Active).HasColumnType("tinyint(1)");
			modelBuilder.Entity<CustomerAccount>().Property(ca => ca.MarkAsDefault).HasColumnType("tinyint(1)");
			// modelBuilder.Entity<CustomerAccount>().Property(ca => ca.BankAccount).HasColumnType("varchar(10)");

			modelBuilder.Entity<CustomerAccount>().HasKey(c => new { c.CustomerId, c.BankAccount });
			modelBuilder.Entity<Customer>().HasMany(c => c.BankAccounts);


			//modelBuilder.Entity<CustomerAccount>()
			//	.HasOne(ca => ca.Customer)
			//	.WithMany(c => c.BankAccounts)
			//	.HasForeignKey(ca => ca.CustomerId);

			// Populate sample data
			modelBuilder.Entity<Customer>().HasData(
				Customer.CreateNew(1, "Juan Luis", "Guerrero Minero"),
				Customer.CreateNew(2, "Francisco", "Ruiz Vázquez"),
				Customer.CreateNew(3, "Eva", "Perez Moreno"),
				Customer.CreateNew(4, "Maria", "Serrano Sanchez")
			);

			modelBuilder.Entity<CustomerAccount>().HasData(
				CustomerAccount.CreateNew(1, "ES12-1234-1234-1234567890", true),
				CustomerAccount.CreateNew(1, "ES13-4321-4321-0987654321", false)
			);
		}
	}
}
