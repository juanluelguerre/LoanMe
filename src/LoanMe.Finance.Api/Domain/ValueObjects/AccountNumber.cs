using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoanMe.Finance.Api.Domain.ValueObjects
{
	public sealed class AccountNumber : ValueObject // : IEquatable<AccountNumber>
	{
		[MaxLength(4)]
		public string Entity { get; private set; }
		[MaxLength(4)]
		public string Office { get; private set; }
		[MaxLength(2)]
		public string Control { get; private set; }
		[MaxLength(10)]
		public string Number { get; private set; }

		public AccountNumber(string entity, string office, string control, string number)
		{
			Entity = entity;
			Office = office;
			Control = control;
			Number = number;

			if (entity == null || office == null || control == null || number == null)
			{
				throw new ArgumentException($"Invalid account number !");
			}

			if (entity.Length < 4 || office.Length < 4 || control.Length < 2 || number.Length < 0)
			{
				throw new ArgumentException($"Invalid length for account number !");
			}
		}

		public override string ToString()
		{
			return $"{Entity}-{Office}-{Control}-{Number}";
		}

		//public override bool Equals(object obj)
		//{
		//	if (ReferenceEquals(null, obj))
		//	{
		//		return false;
		//	}

		//	if (ReferenceEquals(this, obj))
		//	{
		//		return true;
		//	}

		//	return Equals(obj as AccountNumber);
		//}

		//public override int GetHashCode()
		//{
		//	unchecked
		//	{
		//		int hash = 137;
		//		hash = hash * 23 + Entity.GetHashCode();
		//		hash = hash * 23 + Office.GetHashCode();
		//		hash = hash * 23 + Control.GetHashCode();
		//		hash = hash * 23 + Number.GetHashCode();
		//		return hash;
		//	}
		//}

		//public bool Equals(AccountNumber other)
		//{
		//	return other.Entity == Entity
		//		&& other.Office == Office
		//		&& other.Control == Control
		//		&& other.Number == Number;
		//}

		protected override IEnumerable<object> GetAtomicValues()
		{
			// Using a yield return statement to return each element one at a time
			yield return Entity;
			yield return Office;
			yield return Control;
			yield return Number;			
		}
	}
}
