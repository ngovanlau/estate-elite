using Common.Application.Responses;
using MediatR;

namespace PropertyService.Application.Requests.Properties;

public sealed record GetPropertyDetailsRequest : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
}
