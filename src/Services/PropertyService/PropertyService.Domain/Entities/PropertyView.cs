using SharedKernel.Entities;

namespace PropertyService.Domain.Entities;

public class PropertyView : AuditableEntity
{
    public Guid? UserId { get; set; }
    public required string IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public Guid PropertyId { get; set; }
}
