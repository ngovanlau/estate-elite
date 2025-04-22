using PropertyService.Domain.Entities;
using SharedKernel.Interfaces;

namespace PropertyService.Application.Interfaces;

public interface IImageRepository : IRepository<Image>
{
    Task<bool> AddImagesAsync(List<Image> s, CancellationToken cancellationToken = default);
}
