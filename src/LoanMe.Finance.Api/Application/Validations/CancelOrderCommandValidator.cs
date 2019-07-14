using FluentValidation;
using LoanMe.Finance.Api.Application.Commands;
using Microsoft.Extensions.Logging;

namespace LoanMe.Finance.Api.Application.Validations
{
	public class CancelOrderCommandValidator : AbstractValidator<CancelDepositCommand>
	{
		public CancelOrderCommandValidator(ILogger<CancelOrderCommandValidator> logger)
		{
			RuleFor(order => order.OrderNumber).NotEmpty().WithMessage("No orderId found");

			logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
		}
	}
}
