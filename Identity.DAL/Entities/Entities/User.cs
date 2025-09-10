namespace Identity.DAL.Entities.Entities;

public class User:BaseEntity
{
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public bool Confirmed { get; set; }
}
