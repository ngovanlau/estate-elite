using AutoMapper;
using PaymentService.Application.Dtos;
using PaymentService.Domain.Entities;

namespace PaymentService.Infrastructure.Mappers;

public class TransactionMapperProfile : Profile
{
    public TransactionMapperProfile()
    {
        CreateMap<Transaction, TransactionDto>();
    }
}
