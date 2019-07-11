using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanMe.Catalog.Api.Application.Entities
{
	public class Customer
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string FirstName { get; set; }

		[Required]
		[MaxLength(100)]
		public string LastName { get;  set; }

		public bool Active { get; set; }

		[Required]
		[ForeignKey(nameof(CustomerAccount.CustomerId))]
		public ICollection<CustomerAccount> BankAccounts { get; set; } 
			= new List<CustomerAccount>();
	}
}
