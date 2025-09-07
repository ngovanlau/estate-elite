using AutoMapper;
using Common.Infrastructure.Implements;
using PropertyService.Application.Interfaces;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;

namespace PropertyService.Infrastructure.Repositories;

public class ImageRepository(PropertyContext context, IMapper mapper) : Repository<Image>(context, mapper), IImageRepository
{
}
