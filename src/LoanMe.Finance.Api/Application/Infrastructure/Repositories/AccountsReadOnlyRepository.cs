using Dapper;
using LoanMe.Finance.Api.Application.Domain.Aggregates.AccountAggregate;
using LoanMe.Finance.Api.Application.Queries;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace LoanMe.Catalog.Api.Application.Queries
{
	public class AccountsReadOnlyRepository : IAccountsReadOnlyRepository
	{
		private const string BASE_QUERY = 
			@"SELECT c.Id, c.FirstName, c.LastName, ca.BankAccount, ca.MarkAsDefault
			FROM LoanMe.Customers c
			LEFT JOIN LoanMe.dd ca
			ON c.Id = ca.CustomerId
			WHERE ca.BankAccount IS NOT NULL
			AND c.Active";

		private readonly string _connectionString;
		
		public AccountsReadOnlyRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public IDbConnection Connection
		{
			get
			{
				return new SqlConnection(_connectionString);
			}
		}

		public async Task<AccountViewModel> GetCustomerAsync(int id)
		{			
			using (var conn = Connection)
			{
				var result = await conn.QueryFirstOrDefaultAsync<AccountViewModel>($"{BASE_QUERY} AND c.Id = @Id;", id);
				return result;
			}
		}

		public async Task<IEnumerable<AccountViewModel>> GetCustomersAsync()
		{
			using (var conn = Connection)
			{				
				var result = await conn.QueryAsync<AccountViewModel>($"{BASE_QUERY};");
				return result;
			}
		}		
	}
}
