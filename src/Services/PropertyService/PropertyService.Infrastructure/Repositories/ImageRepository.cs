using PropertyService.Application.Interfaces;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;
using SharedKernel.Implements;

namespace PropertyService.Infrastructure.Repositories;

public class ImageRepository(PropertyContext context) : Repository<Image>(context), IImageRepository
{
    public async Task<bool> AddImagesAsync(List<Image> images, CancellationToken cancellationToken)
    {
        await context.AddRangeAsync(images, cancellationToken);
        return await SaveChangeAsync(cancellationToken);
    }
}