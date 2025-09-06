using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

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
