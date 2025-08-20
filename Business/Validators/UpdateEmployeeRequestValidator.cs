using Business.Requests;
using FluentValidation;

namespace Business.Validators;

public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .When(x => x.Name is not null);

        RuleFor(x => x.Surname)
            .NotEmpty()
            .When(x => x.Surname is not null);

        RuleFor(x => x.Phone)
            .MustBeValidPhoneNumber()
            .When(x => x.Phone is not null);

        RuleFor(x => x.CompanyId)
            .GreaterThan(0)
            .When(x => x.CompanyId.HasValue);
        
        RuleFor(x => x.Passports)
            .NotNull()
            .When(x => x.Passports != null)
            .DependentRules(() =>
            {
                RuleForEach(x => x.Passports)
                    .SetValidator(new PassportDtoValidator());
            });

        RuleFor(x => x.DepartmentName)
            .NotEmpty()
            .When(x => !string.IsNullOrWhiteSpace(x.DepartmentName));
    }
}