using System;
using SharedKernel.Entities;
using SharedKernel.Enums;

namespace PaymentService.Domain.Entities;

public class Transaction : AuditableEntity
{
    public decimal Amount { get; set; }
    public CurrencyUnit CurrencyUnit { get; set; }
    public TransactionStatus Status { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public Guid UserId { get; set; }
    public Guid PropertyId { get; set; }
}
