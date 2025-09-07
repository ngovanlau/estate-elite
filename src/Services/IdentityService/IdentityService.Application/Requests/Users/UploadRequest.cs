using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IdentityService.Application.Requests.Users;

using Common.Application.Responses;

public class UploadRequest : IRequest<ApiResponse>
{
    public required IFormFile Image { get; set; }

    [JsonIgnore]
    public bool IsAvatar { get; private set; }

    public void SetIsAvatar(bool value)
    {
        IsAvatar = value;
    }
}
