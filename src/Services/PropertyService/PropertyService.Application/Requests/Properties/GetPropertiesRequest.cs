using MediatR;
using SharedKernel.Requests;
using SharedKernel.Responses;

namespace PropertyService.Application.Requests.Properties;

public class GetPropertiesRequest : PageRequest, IRequest<PageApiResponse>
{
    public string? Address { get; set; }
    public Guid? PropertyTypeId { get; set; }
}