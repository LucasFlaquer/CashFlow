using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRegisterExpenseJsonBuilder
{
    public RequestExpenseJson Build()
    {
        return new Faker<RequestExpenseJson>()
            .RuleFor(request => request.Title, faker => faker.Commerce.ProductName())
            .RuleFor(request => request.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(request => request.Date, faker => faker.Date.Past())
            .RuleFor(request => request.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(request => request.Amount, faker => faker.Random.Decimal(min: 1, max: 1000));
    }
}
