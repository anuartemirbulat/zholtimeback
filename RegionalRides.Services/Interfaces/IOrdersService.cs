using RegionalRides.DataContracts.Orders;

namespace RegionalRides.Services.Interfaces;

public interface IOrdersService
{
    Task<bool> Create(CreateOrderRequest request);
    Task<OrderResponse[]> Get(int? sourceKatoId,int? destinationKatoId, DateTime? departureDate);
    Task<OrderResponse[]> GetOrdersByCurrentProfile();
    Task<bool> Cancel(int orderId);
}
