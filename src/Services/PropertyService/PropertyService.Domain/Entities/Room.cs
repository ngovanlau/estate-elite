
using SharedKernel.Entities;

namespace PropertyService.Domain.Entities;

public class Room : AuditableEntity
{
    public required string Name { get; set; }

    public ICollection<PropertyRoom> PropertyRooms { get; set; } = default!;
}
