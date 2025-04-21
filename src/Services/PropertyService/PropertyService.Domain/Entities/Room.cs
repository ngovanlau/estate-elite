
using SharedKernel.Entities;

namespace PropertyService.Domain.Entities;

public class Room : AuditableEntity
{
    public required string Name { get; set; }

    public ICollection<Property> Properties { get; set; } = default!;
}
