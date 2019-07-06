using System.Linq;

namespace LoanMe.Catalog.Api.Application.Data
{
	public class DbInitializer
	{
		public static void Initialize(DataContext context)
		{
			//context.Database.EnsureCreated();

			// Look for any students.
			//if (context.Finances.Any())
			//{
			//	return;   // DB has been seeded
			//}			
		}
	}
}
