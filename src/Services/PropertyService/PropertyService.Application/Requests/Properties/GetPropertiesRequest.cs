using MediatR;
using SharedKernel.Responses;

namespace PropertyService.Application.Requests.Properties;

public class GetPropertiesRequest : IRequest<ApiResponse>
{
}