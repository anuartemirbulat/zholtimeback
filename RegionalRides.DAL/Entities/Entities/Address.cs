using RegionalRides.DAL.Entities.References;

namespace RegionalRides.DAL.Entities.Entities;

public class Address : BaseEntity
{
    public int KatoId { get; set; }
    public virtual RefKato Kato { get; set; }
    public string Street { get; set; }
    public string House { get; set; }

    public Address()
    {
    }

    public Address(int katoId, string street, string house)
    {
        this.KatoId = katoId;
        Street = street;
        House = house;
    }
}
