using Constants.Enums;

namespace RegionalRides.DataContracts.Orders;

public class OrderResponse
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; }
    public string InitialName { get; set; }
    public string Nickname { get; set; }

    public AddressDto Source { get; set; }
    public AddressDto Destination { get; set; }
    public decimal Price { get; set; }
    public OrderStateEnum State { get; set; }
    public DateTime DepartureDateTime { get; set; }
    public DateTime CreateDateTime { get; set; }
}
