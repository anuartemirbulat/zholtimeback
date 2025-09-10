namespace Identity.DAL;

public interface IEntity
{
    long Id { get; set; }
    DateTime DateCreate { get; set; }
    DateTime DateUpdate { get; set; }
}
