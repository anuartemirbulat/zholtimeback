namespace RegionalRides.DataContracts.Orders;

public class CreateOrderRequest
{
    public AddressDto Source { get; set; }
    public AddressDto Destination { get; set; }
    public decimal Price { get; set; }
    public string Comment { get; set; }
    public DateTime DepartureDateTime { get; set; }
}
