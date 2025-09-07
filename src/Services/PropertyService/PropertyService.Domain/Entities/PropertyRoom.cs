namespace PropertyService.Domain.Entities;

public class PropertyRoom
{
    public int Quantity { get; set; }

    public Guid PropertyId { get; set; }
    public required Property Property { get; set; }

    public Guid RoomId { get; set; }
    public required Room Room { get; set; }
}
