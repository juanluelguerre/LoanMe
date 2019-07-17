namespace LoanMe.Catalog.Api
{
	public class CatalogSettings
	{
		public string PicBaseUrl { get; set; }
		public string EventBusConnection { get; set; }
		public bool UseCustomizationData { get; set; }
		public bool AzureServiceBusEnabled { get; set; }
		public bool AzureStorageEnabled { get; set; }
		public string SubscriptionClientName { get; set; }
		public int EventBusRetryCount { get; set; }
		public bool UseVault { get; set; }

	}
}
