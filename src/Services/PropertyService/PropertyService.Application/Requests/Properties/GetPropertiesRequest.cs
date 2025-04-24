using MediatR;
using SharedKernel.Requests;
using SharedKernel.Responses;

namespace PropertyService.Application.Requests.Properties;

public class GetPropertiesRequest : PageRequest, IRequest<PageApiResponse>
{
}