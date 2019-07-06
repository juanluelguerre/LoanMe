using System.Linq;

namespace LoanMe.Catalog.Api.Application.Entities
{
	public class DbInitializer
	{
		public static void Initialize(DataContext context)
		{
			//context.Database.EnsureCreated();

			// Look for any students.
			//if (context.Customers.Any())
			//{
			//	return;   // DB has been seeded
			//}

			// TODO: Add Seeds for customers/Banks/.....
		}
	}
}
