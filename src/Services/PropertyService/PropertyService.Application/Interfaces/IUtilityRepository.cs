namespace PropertyService.Application.Interfaces;

using PropertyService.Application.Dtos.Utilities;
using PropertyService.Domain.Entities;
using SharedKernel.Interfaces;

public interface IUtilityRepository : IRepository<Utility>
{
    Task<List<UtilityDto>> GetAllUtilityDtoAsync(CancellationToken cancellationToken = default);
    Task<List<Utility>> GetUtilityByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default);
}
