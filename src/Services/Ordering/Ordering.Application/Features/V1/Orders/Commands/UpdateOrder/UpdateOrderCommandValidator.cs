using FluentValidation;

namespace Ordering.Application.Features.V1.Orders;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
	public UpdateOrderCommandValidator()
	{
		RuleFor(expression: p => p.UserName)
		.NotEmpty().WithMessage("{UserName} is required.")
		.NotNull()
		.MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

		RuleFor(expression: p => p.EmailAddress)
		.EmailAddress().WithMessage("{EmailAddress} is valid format.")
		.NotEmpty().WithMessage("{EmailAddress} is required.");

		RuleFor(expression: p => p.TotalPrice)
			.NotEmpty().WithMessage("{TotalPrice} is required.")
			.GreaterThan(valueToCompare: 0).WithMessage("{TotalPrice} should be greater than zero");
	}
}
