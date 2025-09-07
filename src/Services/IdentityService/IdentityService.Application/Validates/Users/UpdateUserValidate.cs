using FluentValidation;

namespace IdentityService.Application.Validates.Users;

using Requests.Users;
using Common.Domain.Constants;
using Common.Application.Validators;
using static Common.Domain.Constants.ErrorCode;

public class UpdateUserValidate : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserValidate()
    {
        When(x => !string.IsNullOrEmpty(x.Email), () => RuleFor(x => x.Email).Cascade(CascadeMode.Stop).EmailRule());

        When(x => !string.IsNullOrEmpty(x.FullName), () =>
            RuleFor(x => x.FullName!).Cascade(CascadeMode.Stop).MinimumLengthRule(3, "FullName").MaximumLengthRule(30, "FullName"));

        When(x => !string.IsNullOrEmpty(x.Email), () => RuleFor(x => x.Email).Cascade(CascadeMode.Stop).EmailRule());

        When(x => !string.IsNullOrEmpty(x.Email), () => RuleFor(x => x.Email).Cascade(CascadeMode.Stop).EmailRule());

        RuleFor(x => x.Phone)
            .Matches(RegexPatterns.Phone).When(x => !string.IsNullOrEmpty(x.Phone))
            .WithErrorCode(nameof(E111))
            .WithMessage(E111);

        When(x => !string.IsNullOrEmpty(x.FullName), () => RuleFor(x => x.Address!).MaximumLengthRule(200, "Address"));
    }

}