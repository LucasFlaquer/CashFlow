using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase: IRegisterExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterExpenseUseCase(
        IExpensesWriteOnlyRepository repository, 
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public ResponseRegisterExpenseJson Execute(RequestExpenseJson request)
    {
        Validate(request);
        var entity = _mapper.Map<Expense>(request);
        //var entity = new Expense
        //{
        //    Title = request.Title,
        //    Description = request.Description,
        //    Date = request.Date,
        //    Amount = request.Amount,
        //    PaymentType = (Domain.Enums.PaymentType)request.PaymentType
        //};
        _repository.Add(entity);
        _unitOfWork.Commit();
        return _mapper.Map<ResponseRegisterExpenseJson>(entity);
        //return new ResponseRegisterExpenseJson();
    }

    private void Validate(RequestExpenseJson request)
    {
        var validator = new RegisterExpenseValidator();
        var result = validator.Validate(request);
        if (result.IsValid) return;
        var erroMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
        throw new ErrorOnValidationException(erroMessages);
    }
}
