namespace PropertyService.Application.Interfaces;

using PropertyService.Application.Dtos.PropertyTypes;
using PropertyService.Domain.Entities;
using SharedKernel.Interfaces;

public interface IPropertyTypeRepository : IRepository<PropertyType>
{
    Task<List<PropertyTypeDto>> GetAllPropertyTypeDtoAsync(CancellationToken cancellationToken = default);
}
