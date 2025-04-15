namespace SharedKernel.Entities;

public abstract class AuditableEntity : Entity
{
    public DateTime CreatedOn { get; init; }
    public Guid CreatedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public Guid? ModifiedBy { get; set; }
    public bool IsDelete { get; set; }

    public AuditableEntity()
    {
        CreatedOn = DateTime.UtcNow;
    }

    public AuditableEntity(Guid createdBy) : this()
    {
        CreatedBy = createdBy;
    }
}
