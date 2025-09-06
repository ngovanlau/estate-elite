using MediatR;
using Common.Application.Responses;

namespace PropertyService.Application.Requests.Properties;

public record TrackViewRequest(
    Guid PropertyId,
    string IpAddress,
    string? UserAgent) : IRequest;