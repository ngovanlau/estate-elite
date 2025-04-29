using SharedKernel.Entities;

namespace PropertyService.Domain.Entities;

public class PropertyRental : AuditableEntity
{
    public Guid UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public Guid PropertyId { get; set; }
    public Property? Property { get; set; }
}

