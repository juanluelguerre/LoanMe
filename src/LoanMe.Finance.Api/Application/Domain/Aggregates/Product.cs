//using System;
//using System.ComponentModel.DataAnnotations;

//namespace LoanMe.Finance.Api.Application.Domain.Aggregates
//{
//	public class Product : AggregateRoot
//	{
//		[Key]
//		public int Id { get; private set; }
//		public string Name { get; private set; }
//		public string Description { get; private set; }
//		public double Prize { get; private set; }
//		public CurrencyType Currency {get; private set;}
		
//		public Product(int id, string name, string description, double prize)
//		{
//			Id = id;
//			Name = name ?? throw new ArgumentNullException(nameof(name));
//			Description = description ?? throw new ArgumentNullException(nameof(description));
//			Prize = prize;
//			Currency = CurrencyType.EUR;
//		}
//	}
//}
