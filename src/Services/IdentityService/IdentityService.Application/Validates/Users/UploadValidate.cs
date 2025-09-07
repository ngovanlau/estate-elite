using Common.Application.Validators;
using FluentValidation;
using IdentityService.Application.Requests.Users;

namespace IdentityService.Application.Validates.Users;

public class UploadValidate : AbstractValidator<UploadRequest>
{
    public UploadValidate()
    {
        RuleFor(x => x.Image).Cascade(CascadeMode.Stop).ImageRule();
    }
}
