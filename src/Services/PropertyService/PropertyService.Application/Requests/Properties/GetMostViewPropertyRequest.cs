using MediatR;
using Common.Application.Responses;

namespace PropertyService.Application.Requests.Properties;

public sealed record GetMostViewPropertiesRequest(int Quantity) : IRequest<ApiResponse>;
