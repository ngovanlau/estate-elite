using Common.Application.Requests;
using Common.Application.Responses;
using MediatR;

namespace PropertyService.Application.Requests.Properties;

public class GetOwnerPropertiesRequest : PageRequest, IRequest<PageApiResponse>
{
}
