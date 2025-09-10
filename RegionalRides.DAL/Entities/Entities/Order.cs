using Constants.Enums;

namespace RegionalRides.DAL.Entities.Entities;

public class Order : BaseEntity
{
    public virtual Address SourceAddress { get; set; }
    public int SourceAddressId { get; set; }
    public virtual Address DestinationAddress { get; set; }
    public int DestinationAddressId { get; set; }
    public virtual Profile Customer { get; set; }
    public int CustomerId { get; set; }
    public decimal Price { get; set; }
    public string Comment { get; set; }
    public DateTime DepartureDateTime { get; set; }
    public OrderStateEnum State { get; set; }
}
