﻿using CashFlow.Application.UseCases.Expenses;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validators.Tests.Expenses.Register;

public class RegisterExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        var result = validator.Validate(request);
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Error_Title_Empty(string title)
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Title = title;
        var result = validator.Validate(request);
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe(ResourceErrorMessages.TITLE_REQUIRED);
    }

    [Fact]
    public void Error_Date_Future()
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(1);
        var result = validator.Validate(request);
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe(ResourceErrorMessages.DATE_CANNOT_FOR_THE_FUTURE);
    }

    [Fact]
    public void Error_Payment_Type_Invalid()
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.PaymentType = (PaymentType)9314;
        var result = validator.Validate(request);
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe(ResourceErrorMessages.PAYMENT_TYPE_INVALID);
    }
    
    //**Equivalente ao it.each do jest**
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Error_Amount_Invalid(decimal amount)
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Amount = amount;
        var result = validator.Validate(request);
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
    }
}
