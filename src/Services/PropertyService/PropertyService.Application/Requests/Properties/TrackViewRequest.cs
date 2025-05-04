using MediatR;
using SharedKernel.Responses;

namespace PropertyService.Application.Requests.Properties;

public record TrackViewRequest(
    Guid PropertyId,
    string IpAddress,
    string? UserAgent) : IRequest;