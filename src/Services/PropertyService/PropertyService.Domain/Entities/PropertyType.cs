using SharedKernel.Entities;

namespace PropertyService.Domain.Entities;

public class PropertyType : AuditableEntity
{
    public required string Name { get; set; }
}
