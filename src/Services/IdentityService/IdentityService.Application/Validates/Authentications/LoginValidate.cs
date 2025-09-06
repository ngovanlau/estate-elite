using FluentValidation;

namespace IdentityService.Application.Validates.Authentications;

using IdentityService.Application.Interfaces;
using Requests.Authentications;
using Common.Application.Validators;
using static SharedKernel.Constants.ErrorCode;


public class LoginValidate : AbstractValidator<LoginRequest>
{
    private readonly IUserRepository _repository;

    public LoginValidate(IUserRepository repository)
    {
        _repository = repository;

        RuleFor(p => p)
            .Must(p => !(string.IsNullOrWhiteSpace(p.Username) && string.IsNullOrWhiteSpace(p.Email)))
            .WithErrorCode(nameof(E113)).WithMessage(E113);

        When(p => p.Username != null, () =>
        {
            RuleFor(p => p.Username)
                .Cascade(CascadeMode.Stop)
                .UsernameRule()
                .MustAsync((username, cancellationToken) => _repository.IsUsernameExistAsync(username!, cancellationToken))
                .WithErrorCode(nameof(E008)).WithMessage(string.Format(E008, "User"));
        });

        When(p => p.Email != null, () =>
        {
            RuleFor(p => p.Email)
                .Cascade(CascadeMode.Stop)
                .EmailRule()
                .MustAsync((email, cancellationToken) => _repository.IsEmailExistAsync(email!, cancellationToken))
                .WithErrorCode(nameof(E008)).WithMessage(string.Format(E008, "User"));
        });

        RuleFor(p => p.Password).PasswordRule();
    }
}
