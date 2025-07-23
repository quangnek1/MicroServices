using FluentValidation;

namespace Ordering.Application.Features.V1.Orders;

public class CreateOrUpdateValidator : AbstractValidator<CreateOrUpdateCommand>
{
	public CreateOrUpdateValidator()
	{
		RuleFor(expression: p => p.FirstName)
			.NotEmpty().WithMessage("First name is required.")
			.NotNull()
			.MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

		RuleFor(expression: p => p.LastName)
			.NotEmpty().WithMessage("Last name is required.")
			.NotNull()
			.MaximumLength(150).WithMessage("Last name must not exceed 150 characters.");

		RuleFor(expression: p => p.EmailAddress)
		.EmailAddress().WithMessage("{EmailAddress} email address format.")
		.NotEmpty().WithMessage("Email address is required.");

		RuleFor(expression: p => p.TotalPrice)
			.NotEmpty().WithMessage("Total price is required.")
			.GreaterThan(0).WithMessage("Total price must be greater than zero.");

	}
}
