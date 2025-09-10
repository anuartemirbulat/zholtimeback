using Constants.Enums;

namespace Identity.DAL.Entities.Entities;

public class RefRole : BaseEntity
{
    public string Name { get; set; }
    public RoleEnum RoleEnum { get; set; }
}
