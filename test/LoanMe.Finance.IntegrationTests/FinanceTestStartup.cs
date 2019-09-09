using LoanMe.Finance.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace LoanMe.Finance.IntegrationTests
{
	public class FinanceTestsStartup : Startup
	{
		public FinanceTestsStartup(IConfiguration env) : base(env)
		{
		}

		protected override void ConfigureAuth(IApplicationBuilder app)
		{
			if (Configuration["isTest"] == bool.TrueString.ToLowerInvariant())
			{
				app.UseMiddleware<AutoAuthorizeMiddleware>();
			}
			else
			{
				base.ConfigureAuth(app);
			}
		}
	}
}
