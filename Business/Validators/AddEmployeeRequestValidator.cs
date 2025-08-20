using Business.Requests;
using FluentValidation;

namespace Business.Validators;

public class AddEmployeeRequestValidator : AbstractValidator<AddEmployeeRequest>
{
    public AddEmployeeRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Surname).NotEmpty();
        RuleFor(x => x.Phone).MustBeValidPhoneNumber();
        RuleFor(x => x.CompanyId).GreaterThan(0);
        RuleFor(x => x.PassportDto).SetValidator(new PassportDtoValidator());
        RuleFor(x => x.DepartmentName).NotEmpty();
    }
}