namespace LoanMe.Finance.Api.Domain.Aggregates
{
	public class Entity
	{
		private int _id;

		public virtual int Id
		{
			get => _id;
			protected set
			{
				_id = value;
			}
		}
	}
}
