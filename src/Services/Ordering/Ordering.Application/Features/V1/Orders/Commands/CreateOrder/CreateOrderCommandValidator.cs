using FluentValidation;

namespace Ordering.Application.Features.V1.Orders;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
	public CreateOrderCommandValidator()
	{
		RuleFor(expression: p => p.UserName)
		.NotEmpty().WithMessage("{UserName} is required")
		.NotNull()
		.MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters");

		RuleFor(expression: email => email.EmailAddress)
			.EmailAddress().WithMessage("{EmailAddress} is invalid format.")
			.NotEmpty().WithMessage("{EmailAddress} is required.");

		RuleFor(expression: p => p.TotalPrice)
			.NotEmpty().WithMessage("{TotalPrice} is required")
			.GreaterThan(valueToCompare: 0).WithMessage("{TotalPrice} should be greater than zero");
	}
}
