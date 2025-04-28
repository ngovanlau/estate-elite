using AutoMapper;
using PropertyService.Application.Interfaces;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;
using SharedKernel.Implements;

namespace PropertyService.Infrastructure.Repositories;

public class PropertyRentalRepository(
    PropertyContext context,
    IMapper mapper) : Repository<PropertyRental>(context, mapper), IPropertyRentalRepository
{
}
