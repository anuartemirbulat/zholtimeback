using Services.Interfaces;

namespace Identity.DAL.Entities.Entities;

public class Profile : BaseEntity, IPrototype<Profile>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string InitialName { get; set; }
    public string Nickname { get; set; }
    public virtual User User { get; set; }
    public long UserId { get; set; }
    public Guid Guid { get; set; }
    public virtual RefRole Role { get; set; }
    public long RoleId { get; set; }

    public Profile Clone()
    {
        return new Profile()
        {
            FirstName = this.FirstName,
            LastName = this.LastName,
            MiddleName = this.MiddleName,
            InitialName = this.InitialName,
            Nickname = this.Nickname,
        };
    }
}
