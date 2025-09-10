namespace RegionalRides.DAL;

public interface IEntity
{
    int Id { get; set; }
    DateTime DateCreate { get; set; }
    DateTime DateUpdate { get; set; }
}
