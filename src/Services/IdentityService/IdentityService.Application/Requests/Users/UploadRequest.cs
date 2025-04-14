using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace IdentityService.Application.Requests.Users;

using SharedKernel.Commons;

public class UploadRequest : IRequest<ApiResponse>
{
    public required IFormFile Image { get; set; }

    [JsonIgnore]
    public bool IsAvatar { get; set; }
}
