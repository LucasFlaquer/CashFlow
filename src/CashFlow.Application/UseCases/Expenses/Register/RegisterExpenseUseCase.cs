using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase
{
    public ResponseRegisterExpenseJson Execute(RequestExpenseJson request)
    {
        Validate(request);
        return new ResponseRegisterExpenseJson();
    }

    private void Validate(RequestExpenseJson request)
    {
        var titleIsEmpty = string.IsNullOrWhiteSpace(request.Title);
        if (titleIsEmpty)
        {
            throw new ArgumentException("Title cannot be empty.");
        }
        if (request.Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.");
        }
        var result = DateTime.Compare(request.Date, DateTime.UtcNow);
        var isFutureDate = result > 0;
        if (isFutureDate)
        {
            throw new ArgumentException("Date must be a valid date.");
        }
        var isPaymentTypeValid = Enum.IsDefined(typeof(PaymentType), request.PaymentType);
        if (!isPaymentTypeValid) 
        {
            throw new ArgumentException("Payment type is not valid.");
        }
    }
}
