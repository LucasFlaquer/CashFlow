using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace Validators.Tests.Expenses.Register;

public class RegisterExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        var validator = new RegisterExpenseValidator();
        var request = new RequestExpenseJson
        {
            Title = "Test Expense",
            Amount = 100.0m,
            Date = DateTime.UtcNow.AddDays(-1),
            PaymentType = PaymentType.Cash
        };
        var result = validator.Validate(request);
        Assert.True(result.IsValid);
    }
}
