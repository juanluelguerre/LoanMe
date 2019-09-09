﻿using LoanMe.Finance.Api.Domain.Aggregates;
using LoanMe.Finance.Api.Domain.Aggregates.CustomerAggregate;
using LoanMe.Finance.Api.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;

namespace LoanMe.Catalog.Api.Application.Data
{
	public class FinanceContext : DbContext
	{
		public const string TABLE_ACCOUNTS = "Accounts";		
		public const string TABLE_LOANS = "Loans";

		public FinanceContext(DbContextOptions<FinanceContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Account>().ToTable(TABLE_ACCOUNTS);			
			modelBuilder.Entity<Loan>().ToTable(TABLE_LOANS);

			// Bug: MySQL buhttps://stackoverflow.com/questions/47330796/entity-framework-core-with-mysql-unknown-column-in-field-list
			//		Also not suportend ICollection<> properties using "Pomelo..." Nuget.
			//		modelBuilder.Entity<Customer>().Property(a => a.BankAccounts).HasColumnName("BankAccount");

			// modelBuilder.Entity<Product>().HasKey(c => new { c.Id });
			// modelBuilder.Entity<Product>().HasKey(c => new { c.Id });
			// modelBuilder.Entity<Loan>().HasOne(c => c.Product);

			//var p1 = new Product(1, "Car", "The more beautiful car in the world", 100000.00);
			//var p2 = new Product(2, "House", "My Future big house", 250000.00);
			//var p3 = new Product(3, "Computer", "Nice latest invisible laptop", 21000.00);

			// Master data
			// modelBuilder.Entity<Product>().HasData(p1, p2, p3);

			// Master data
			modelBuilder.Entity<Loan>().HasData(
				new Loan(1, 1,  new AccountNumber("ES12", "1234", "12", "12345678")),
				new Loan(2, 2, new AccountNumber("ES12", "1234", "12", "12345678")),
				new Loan(3, 3, new AccountNumber("ES12", "4321", "21", "87654321"))
			);

			// Sample data
			modelBuilder.Entity<Account>().HasData(
				new Account(1, new AccountNumber("ES12", "1234", "12", "12345678"), new Customer("Juan Luis Guerrero"), new CreditCard(1, DateTime.Now, CardType.Visa, 1000)),
				new Account(2, new AccountNumber("ES12", "1234", "12", "87654321"), new Customer("Pedro Ruiz"), new CreditCard(2, DateTime.Now, CardType.MasterCard, 2000))
			);
		}
	}
}