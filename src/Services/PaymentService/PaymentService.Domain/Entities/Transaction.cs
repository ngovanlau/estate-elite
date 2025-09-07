using System;
using Common.Domain.Enums;
using SharedKernel.Entities;

namespace PaymentService.Domain.Entities;

public class Transaction : AuditableEntity
{
    public double Amount { get; set; }
    public CurrencyUnit CurrencyUnit { get; set; }
    public TransactionStatus Status { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public Guid UserId { get; set; }
    public Guid PropertyId { get; set; }
    public required string OrderId { get; set; }
    public int RentalPeriod { get; set; }
}
