namespace LoanMe.Customers.Api.Application.Domain.Interfaces
{
	public interface IDataService<T>
	{
		bool Add(T entity);
		bool Update(T entity);
		bool Delete(int id);
		int ExecuteQuery(string query, params object[] parameters);
	}
}