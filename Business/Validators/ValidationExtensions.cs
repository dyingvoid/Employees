using FluentValidation;

namespace Business.Validators;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> MustBeValidPhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .Matches(@"^\+?[1-9]\d{1,14}$");
    }
}