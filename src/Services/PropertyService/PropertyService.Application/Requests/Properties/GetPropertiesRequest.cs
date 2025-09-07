using Common.Application.Requests;
using Common.Application.Responses;
using MediatR;

namespace PropertyService.Application.Requests.Properties;

public class GetPropertiesRequest : PageRequest, IRequest<PageApiResponse>
{
    public string? Address { get; set; }
    public Guid? PropertyTypeId { get; set; }
}
