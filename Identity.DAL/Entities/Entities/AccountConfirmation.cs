namespace Identity.DAL.Entities.Entities;

public class AccountConfirmation : BaseEntity
{
    public string PhoneNumber { get; set; }
    public string Code { get; set; }
    public DateTime ExpirationDateTime { get; set; }
    public bool Confirmed { get; set; }
    public Guid Guid { get; set; }
}
