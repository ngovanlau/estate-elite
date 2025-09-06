using AutoMapper;
using PropertyService.Application.Interfaces;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;
using Common.Infrastructure.Implements;

namespace PropertyService.Infrastructure.Repositories;

public class AddressRepository(
    PropertyContext context,
    IMapper mapper) : Repository<Address>(context, mapper), IAddressRepository
{
}