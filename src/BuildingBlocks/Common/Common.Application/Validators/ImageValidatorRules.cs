using FluentValidation;
using Microsoft.AspNetCore.Http;
using static Common.Domain.Constants.ErrorCode;

namespace Common.Application.Validators;

public static class ImageValidatorRules
{
    public static IRuleBuilderOptions<T, IFormFile> ImageRule<T>(
        this IRuleBuilder<T, IFormFile> rule,
        int maxSizeInMB = 20,
        string[]? allowedContentTypes = null)
    {
        if (allowedContentTypes == null || !allowedContentTypes.Any())
        {
            allowedContentTypes =
            [
                "image/jpeg",
                "image/png",
                "image/gif"
            ];
        }

        int maxSizeInBytes = maxSizeInMB * 1024 * 1024;

        return rule
            .NotNull().WithErrorCode(E002).WithMessage(string.Format(E002, "Image"))
            .Must(file => file != null && file.Length > 0).WithErrorCode(E002).WithMessage(string.Format(E002, "Image"))
            .Must(file => file == null || file.Length <= maxSizeInBytes).WithErrorCode(nameof(E009)).WithMessage(string.Format(E009, $"{maxSizeInMB}MB"))
            .Must(file => file == null || allowedContentTypes.Contains(file.ContentType)).WithErrorCode(nameof(E010)).WithMessage(E010);
    }

}
