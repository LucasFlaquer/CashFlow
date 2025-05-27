using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseValidator: AbstractValidator<RequestExpenseJson>
{
    public RegisterExpenseValidator()
    {
        RuleFor(expense => expense.Title)
            .NotEmpty().WithMessage("The title is required");
        RuleFor(expense => expense.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");
        RuleFor(expense => expense.Date)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Date must be a valid date and not in the future.");
        RuleFor(expense => expense.PaymentType).IsInEnum()
            .WithMessage("Payment type is not valid.");
    }
}
