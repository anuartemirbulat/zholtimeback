namespace RegionalRides.DAL.Entities.Entities;

public class Profile : BaseEntity
{
    public Guid Guid { get; set; }
    public string InitialName { get; set; }
    public string Nickname { get; set; }
    public string PhoneNumber { get; set; }
}
