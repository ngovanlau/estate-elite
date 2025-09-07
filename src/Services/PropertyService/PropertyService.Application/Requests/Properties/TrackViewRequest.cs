using Common.Application.Responses;
using MediatR;

namespace PropertyService.Application.Requests.Properties;

public record TrackViewRequest(
    Guid PropertyId,
    string IpAddress,
    string? UserAgent) : IRequest;
