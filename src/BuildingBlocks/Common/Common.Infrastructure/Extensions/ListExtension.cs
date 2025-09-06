using FluentValidation.Results;
using Common.Application.Dtos;

namespace Common.Infrastructure.Extensions;

public static class ListExtension
{
    public static List<DicDto> ToDic(this List<ValidationFailure> list)
    {
        return list.Select(p => new DicDto
        {
            Key = p.PropertyName,
            Value = p.ErrorMessage
        }).ToList();
    }
}
