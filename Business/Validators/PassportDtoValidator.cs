using Business.Dtos;
using FluentValidation;

namespace Business.Validators;

public class PassportDtoValidator : AbstractValidator<PassportDto>
{
    public PassportDtoValidator()
    {
        RuleFor(x => x.Type).NotEmpty();
        RuleFor(x => x.Number)
            .NotEmpty()
            .Length(9, 10)
            .Matches(@"^\d+$");
    }
}