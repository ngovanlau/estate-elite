using FluentValidation;

namespace IdentityService.Application.Validates.Authentications;

using IdentityService.Application.Interfaces;
using Requests.Authentications;
using SharedKernel.Validators;
using static SharedKernel.Constants.ErrorCode;


public class LoginValidate : AbstractValidator<LoginRequest>
{
    private readonly IUserRepository _repository;

    public LoginValidate(IUserRepository repository)
    {
        _repository = repository;

        RuleFor(p => p)
            .Must(p => !string.IsNullOrWhiteSpace(p.Username) || !string.IsNullOrWhiteSpace(p.Email))
            .WithErrorCode(nameof(E113)).WithMessage(E113);

        When(p => p.Username != null, () =>
        {
            RuleFor(p => p.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmptyOrWhiteSpaceRule("Username")
                .MustAsync((username, cancellationToken) => _repository.IsUsernameExistAsync(username!, cancellationToken))
                .WithErrorCode(nameof(E111)).WithMessage(E111);
        });

        When(p => p.Username != null, () =>
        {
            RuleFor(p => p.Email)
                .Cascade(CascadeMode.Stop)
                .EmailRule()
                .MustAsync((email, cancellationToken) => _repository.IsEmailExistAsync(email!, cancellationToken))
                .WithErrorCode(nameof(E112)).WithMessage(E112);
        });

        RuleFor(p => p.Password).PasswordRule();
    }
}
