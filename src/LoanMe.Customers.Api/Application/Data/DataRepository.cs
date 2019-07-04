using Microsoft.EntityFrameworkCore;
using LoanMe.Customers.Api.Application.Data;
using LoanMe.Customers.Api.Application.Domain.Aggregates;
using LoanMe.Customers.Api.Application.Domain.Interfaces;

namespace LoanMe.Customers.Api.Application.Data
{
	public class DataRepository<T> : IDataService<T> where T: AggregateRoot
	{
		private DataContext _context;

		public DataRepository(DataContext context)
		{
			_context = context;
		}

		public bool Delete(int id)
		{
			T t = _context.Find<T>(id);
			var result = _context.Remove<T>(t);
			return (result.State == EntityState.Deleted);
		}

		public bool Add(T entity)
		{
			_context.Add<T>(entity);
			var result = _context.SaveChanges();
			return result > 0;
		}

		public bool Update(T entity)
		{
			_context.Update<T>(entity);
			var result = _context.SaveChanges();
			return result > 0;
		}

		public int ExecuteQuery(string query, params object[] parameters)
		{
			return _context.Database.ExecuteSqlCommand(query, parameters);
		}
	}
}
