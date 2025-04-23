using MediatR;

namespace PropertyService.Application.Requests.PropertyTypes;

using SharedKernel.Responses;

public class GetPropertyTypeRequest : IRequest<ApiResponse>
{
}
