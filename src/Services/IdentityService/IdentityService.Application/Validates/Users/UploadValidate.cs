using FluentValidation;
using IdentityService.Application.Requests.Users;
using SharedKernel.Validators;

namespace IdentityService.Application.Validates.Users;

public class UploadValidate : AbstractValidator<UploadRequest>
{
    public UploadValidate()
    {
        RuleFor(x => x.Image).Cascade(CascadeMode.Stop).ImageRule();
    }
}