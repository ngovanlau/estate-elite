using MediatR;
using Common.Application.Responses;

namespace PropertyService.Application.Requests.Properties;

public sealed record GetPropertyDetailsRequest : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
}
