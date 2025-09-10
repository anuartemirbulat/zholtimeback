namespace RegionalRides.DAL;

public abstract class BaseEntity : IEntity
{
    public int Id { get; set; }

    public DateTime DateCreate { get; set; } = DateTime.Now;

    public DateTime DateUpdate { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; } = false;
}
