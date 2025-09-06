using MediatR;
using Common.Application.Requests;
using Common.Application.Responses;

namespace PropertyService.Application.Requests.Properties;

public class GetOwnerPropertiesRequest : PageRequest, IRequest<PageApiResponse>
{
}
