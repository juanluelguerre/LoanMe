using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanMe.Finance.Api.Application.Domain.Aggregates
{
	public class Account : AggregateRoot
	{		
		[Key]
		public int Id { get; private set; }
		// public string Name { get; private set; }

		[MaxLength(4)]
		public string Entity { get; private set; }
		[MaxLength(4)]
		public string Office { get; private set; }
		[MaxLength(2)]
		public string Control { get; private set; }		
		[MaxLength(10)]
		public string Number { get; private set; }

		public double Balance { get; set; }

		[NotMapped]
		public string IBAN => $"{Entity}-{Office}-{Control}-{Number}";

		public Account(int id, string entity, string office, string control, string number)
		{
			Id = id;
			Entity = entity;
			Office = office;
			Control = control;
			Number = number;			

			if (!ValidAccountNumber())
			{
				throw new ArgumentException($"Invalid account number: '{IBAN}'");
			}
		}

		public void AddAmount(double amount)
		{
			Balance += amount;
		}

		public void RemoveAmount(double amount)
		{
			Balance -= amount;
		}

		private bool ValidAccountNumber()
		{
			return true;
		}
	}
}

