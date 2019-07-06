using System.ComponentModel.DataAnnotations;

namespace LoanMe.Catalog.Api.Application.Entities
{
	public class CustomerAccount 
	{
		[Key]
		public int CustomerId { get; set; }

		[Required]
		[MaxLength(50)]
		public string BankAccount { get; set; }

		public bool MarkAsDefault { get; set; }
	}
}
