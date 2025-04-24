namespace SharedKernel.Requests;

public abstract class PageRequest
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
    public Guid? LastEntityId { get; set; }
}
