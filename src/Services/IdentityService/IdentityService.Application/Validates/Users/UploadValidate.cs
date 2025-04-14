using FluentValidation;

namespace IdentityService.Application.Validates.Users;

using Requests.Users;
using static SharedKernel.Constants.ErrorCode;

public class UploadValidate : AbstractValidator<UploadRequest>
{
    public UploadValidate()
    {
        RuleFor(x => x.Image)
            .NotNull()
            .WithErrorCode(E002)
            .WithMessage(string.Format(E002, "Image"));

        RuleFor(x => x.Image.Length)
            .LessThanOrEqualTo(20 * 1024 * 1024)
            .WithErrorCode(nameof(E009))
            .WithMessage(string.Format(E009, "20MB"));

        RuleFor(x => x.Image.ContentType)
            .Must(IsImageFile)
            .WithErrorCode(nameof(E010))
            .WithMessage(E010);
    }

    private bool IsImageFile(string contentType)
    {
        var allowedImageTypes = new[]
        {
            "image/jpeg",
            "image/png",
            "image/gif"
        };

        return allowedImageTypes.Contains(contentType);
    }
}