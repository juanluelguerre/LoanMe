using LoanMe.Finance.Api;
using LoanMe.Finance.Api.Domain.Aggregates;
using LoanMe.Finance.Api.Domain.Aggregates.CustomerAggregate;
using LoanMe.Finance.Api.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LoanMe.Catalog.Api.Application.Data
{
	public class FinanceContext : DbContext
	{
		// private readonly IMediator _mediator;
		// private IDbContextTransaction _currentTransaction;

		public const string DEFAULT_SCHEMA = "Finance";
		public DbSet<Account> Accounts { get; set; }
		public DbSet<CreditCard> CreditCards { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Loan> Loans { get; set; }
		public DbSet<Product> Products{ get; set; }

		// public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
		// public bool HasActiveTransaction => _currentTransaction != null;

		public FinanceContext(DbContextOptions<FinanceContext> options) : base(options)
		{
		}

		//public FinanceContext(DbContextOptions<FinanceContext> options, IMediator mediator) : base(options)
		//{
		//	_mediator = mediator;
		//}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			//modelBuilder.ApplyConfiguration(new AccountEntityTypeConfiguration());
			//modelBuilder.ApplyConfiguration(new CreditCardEntityTypeConfiguration());
			//modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
			//modelBuilder.ApplyConfiguration(new LoanEntityTypeConfiguration());
			//modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());

			builder.ApplyConfigurationsFromAssembly(typeof(Startup).Assembly);

			////// Master data
			////builder.Entity<Loan>().HasData(
			////	new Loan(1, new AccountNumber("ES12", "1234", "12", "12345678")),
			////	new Loan(2, new AccountNumber("ES12", "1234", "12", "12345678")),
			////	new Loan(3, new AccountNumber("ES12", "4321", "21", "87654321"))
			////);

			////// Sample data
			////builder.Entity<Account>().HasData(
			////	new Account(new AccountNumber("ES12", "1234", "12", "12345678"), new Customer("Juan Luis Guerrero"), new CreditCard(1, DateTime.Now, CardType.Visa, 1000), 0),
			////	new Account(new AccountNumber("ES12", "1234", "12", "87654321"), new Customer("Pedro Ruiz"), new CreditCard(2, DateTime.Now, CardType.MasterCard, 2000), 0)
			////);
		}

		//public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
		//{
		//	// Dispatch Domain Events collection. 
		//	// Choices:
		//	// A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
		//	// side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
		//	// B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
		//	// You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
		//	await _mediator.DispatchDomainEventsAsync(this);

		//	// After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
		//	// performed through the DbContext will be committed
		//	var result = await base.SaveChangesAsync(cancellationToken);

		//	return true;
		//}

		//public async Task<IDbContextTransaction> BeginTransactionAsync()
		//{
		//	if (_currentTransaction != null) return null;

		//	_currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

		//	return _currentTransaction;
		//}

		//public async Task CommitTransactionAsync(IDbContextTransaction transaction)
		//{
		//	if (transaction == null) throw new ArgumentNullException(nameof(transaction));
		//	if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

		//	try
		//	{
		//		await SaveChangesAsync();
		//		transaction.Commit();
		//	}
		//	catch
		//	{
		//		RollbackTransaction();
		//		throw;
		//	}
		//	finally
		//	{
		//		if (_currentTransaction != null)
		//		{
		//			_currentTransaction.Dispose();
		//			_currentTransaction = null;
		//		}
		//	}
		//}

		//public void RollbackTransaction()
		//{
		//	try
		//	{
		//		_currentTransaction?.Rollback();
		//	}
		//	finally
		//	{
		//		if (_currentTransaction != null)
		//		{
		//			_currentTransaction.Dispose();
		//			_currentTransaction = null;
		//		}
		//	}
		//}

	}

	public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<FinanceContext>
	{
		public FinanceContext CreateDbContext(string[] args)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
			   .SetBasePath(Directory.GetCurrentDirectory())
			   .AddUserSecrets<Startup>()
			   .AddJsonFile("appsettings.json")
			   .Build();

			var builder = new DbContextOptionsBuilder<FinanceContext>();

			builder.UseSqlServer(configuration.GetConnectionString("DataBaseConnection"));

			return new FinanceContext(builder.Options);
		}
	}
}
