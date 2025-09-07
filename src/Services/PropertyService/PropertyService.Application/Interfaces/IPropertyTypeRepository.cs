namespace PropertyService.Application.Interfaces;

using Common.Application.Interfaces;
using PropertyService.Application.Dtos.PropertyTypes;
using PropertyService.Domain.Entities;

public interface IPropertyTypeRepository : IRepository<PropertyType>
{
    Task<List<PropertyTypeDto>> GetAllPropertyTypeDtoAsync(CancellationToken cancellationToken = default);
}
