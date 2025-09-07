using Common.Domain.Enums;
using SharedKernel.Entities;

namespace PropertyService.Domain.Entities;

public class Project : AuditableEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public Guid DeveloperId { get; set; }
    public double TotalArea { get; set; }
    public Guid TotalUnits { get; set; }
    public DateTime CompletionDate { get; set; }
    public ProjectStatus Status { get; set; }

    public Guid AddressId { get; set; }
    public required Address Address { get; set; }
    public ICollection<Image> Images { get; set; } = default!;
}
