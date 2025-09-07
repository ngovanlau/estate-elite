using AutoMapper;
using Common.Infrastructure.Implements;
using PropertyService.Application.Interfaces;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;

namespace PropertyService.Infrastructure.Repositories;

public class AddressRepository(
    PropertyContext context,
    IMapper mapper) : Repository<Address>(context, mapper), IAddressRepository
{
}
