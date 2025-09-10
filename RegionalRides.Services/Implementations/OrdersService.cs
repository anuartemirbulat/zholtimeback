using Constants.Enums;
using Microsoft.EntityFrameworkCore;
using RegionalRides.DAL;
using RegionalRides.DAL.Entities.Entities;
using RegionalRides.DataContracts.Orders;
using RegionalRides.Services.Interfaces;

namespace RegionalRides.Services.Implementations;

public class OrdersService : IOrdersService
{
    private readonly RegionalRidesContext _dbContext;
    private readonly RegRidesCurrentUserService _currentUserService;

    public OrdersService(RegionalRidesContext dbContext, RegRidesCurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Create(CreateOrderRequest req)
    {
        var customer = await _currentUserService.GetCurrentProfile();
        var source = new Address(req.Source.KatoId, req.Source.Street, req.Source.House);
        await _dbContext.Addresses.AddAsync(source);
        var destination = new Address(req.Destination.KatoId, req.Destination.Street, req.Destination.House);
        await _dbContext.Addresses.AddAsync(destination);
        var order = new Order()
        {
            SourceAddress = source,
            DestinationAddress = destination,
            Price = req.Price,
            Comment = req.Comment,
            Customer = customer,
            DepartureDateTime = req.DepartureDateTime
        };
        await _dbContext.AddAsync(order);
        return true;
    }


    public async Task<OrderResponse[]> Get(int? sourceKatoId, int? destinationKatoId, DateTime? departureDate)
    {
        var now = DateTime.Now.AddHours(-5);
        var query = _dbContext.Orders
            .Where(x => x.DepartureDateTime >= now
                        && x.State == OrderStateEnum.Created)
            .AsQueryable();

        if (sourceKatoId.HasValue)
            query = query.Where(x => x.SourceAddress.KatoId == sourceKatoId);

        if (destinationKatoId.HasValue)
            query = query.Where(x => x.DestinationAddress.KatoId == destinationKatoId);

        if (departureDate.HasValue)
            query = query.Where(x => x.DepartureDateTime == departureDate);

        var orders = await query
            .OrderByDescending(x => x.DateCreate)
            .Select(x => new OrderResponse()
            {
                Id = x.Id,
                PhoneNumber = x.Customer.PhoneNumber,
                InitialName = x.Customer.InitialName,
                Nickname = x.Customer.Nickname,
                Price = x.Price,
                State = x.State,
                DepartureDateTime = x.DepartureDateTime,
                CreateDateTime = x.DateCreate,
                Source = new AddressDto()
                {
                    KatoId = x.SourceAddress.KatoId,
                    Street = x.SourceAddress.Street,
                    House = x.SourceAddress.House,
                    KatoName = x.SourceAddress.Kato.NameRu
                },
                Destination = new AddressDto()
                {
                    KatoId = x.DestinationAddress.KatoId,
                    Street = x.DestinationAddress.Street,
                    House = x.DestinationAddress.House,
                    KatoName = x.DestinationAddress.Kato.NameRu
                }
            }).ToArrayAsync();
        return orders;
    }

    public async Task<OrderResponse[]> GetOrdersByCurrentProfile()
    {
        var profile = await _currentUserService.GetCurrentProfile();
        var orders = _dbContext.Orders.Where(x => x.Customer == profile)
            .Select(x => new OrderResponse()
            {
                Id = x.Id,
                PhoneNumber = x.Customer.PhoneNumber,
                InitialName = x.Customer.InitialName,
                Nickname = x.Customer.Nickname,
                Price = x.Price,
                State = x.State,
                Source = new AddressDto()
                {
                    KatoId = x.SourceAddress.KatoId,
                    Street = x.SourceAddress.Street,
                    House = x.SourceAddress.House,
                    KatoName = x.SourceAddress.Kato.NameRu
                },
                Destination = new AddressDto()
                {
                    KatoId = x.DestinationAddress.KatoId,
                    Street = x.DestinationAddress.Street,
                    House = x.DestinationAddress.House,
                    KatoName = x.DestinationAddress.Kato.NameRu
                }
            }).ToArray();
        return orders;
    }

    public async Task<bool> Cancel(int orderId)
    {
        var order = await _dbContext.Orders.Where(x => x.Id == orderId)
            .SingleAsync();
        order.State = OrderStateEnum.Canceled;
        _dbContext.Update(order);
        return true;
    }
}