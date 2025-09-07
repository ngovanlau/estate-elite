using Common.Application.Responses;
using MediatR;

namespace PropertyService.Application.Requests.Properties;

public sealed record GetMostViewPropertiesRequest(int Quantity) : IRequest<ApiResponse>;
