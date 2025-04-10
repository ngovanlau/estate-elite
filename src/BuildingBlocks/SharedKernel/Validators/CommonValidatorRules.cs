using FluentValidation;

namespace SharedKernel.Validators;

using SharedKernel.Constants;
using static SharedKernel.Constants.ErrorCode;

public static class CommonValidatorRules
{
    public static IRuleBuilder<T, string?> NotEmptyOrWhiteSpaceRule<T>(this IRuleBuilder<T, string?> rule, string propertyName)
    {
        return rule.Must(x => !string.IsNullOrWhiteSpace(x)).WithErrorCode(nameof(E001)).WithMessage(string.Format(E001, propertyName));
    }

    public static IRuleBuilder<T, string?> NotNullRule<T>(this IRuleBuilder<T, string?> rule, string propertyName)
    {
        return rule.NotNull().WithErrorCode(nameof(E002)).WithMessage(string.Format(E002, propertyName));
    }

    public static IRuleBuilder<T, string> MinimumLengthRule<T>(this IRuleBuilder<T, string> rule, int minimumLength, string propertyName)
    {
        return rule.MaximumLength(minimumLength)
            .WithErrorCode(nameof(E004))
            .WithMessage(string.Format(E004, propertyName, minimumLength));
    }

    public static IRuleBuilder<T, string> MaximumLengthRule<T>(this IRuleBuilder<T, string> rule, int maximumLength, string propertyName)
    {
        return rule.MaximumLength(maximumLength)
            .WithErrorCode(nameof(E005))
            .WithMessage(string.Format(E005, propertyName, maximumLength));
    }

    public static IRuleBuilder<T, string?> EmailRule<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.NotEmptyOrWhiteSpaceRule("Email")
            .EmailAddress().WithErrorCode(nameof(E003)).WithMessage(E003);
    }

    public static IRuleBuilder<T, string> PasswordRule<T>(this IRuleBuilder<T, string> rule)
    {
        var name = "Password";

        return rule.MinimumLengthRule(6, name)
            .MaximumLengthRule(256, name)
            .Matches(RegexPatterns.Password).WithErrorCode(E006).WithMessage(E006);
    }
}
