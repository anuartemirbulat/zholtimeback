using Constants.Enums;
using DataContracts.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using RegionalRides.DAL;

namespace RegionalRides.Api.Consumers;

public class DeleteProfileConsumer : IConsumer<DeleteProfileMessage>
{
    private readonly RegionalRidesContext _dbContext;

    public DeleteProfileConsumer(RegionalRidesContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<DeleteProfileMessage> context)
    {
        var profiles = _dbContext.Profiles.Where(x => context.Message.ProfileGuids.Contains(x.Guid)).ToArray();
        var orders = await _dbContext.Orders
            .Where(x => x.State == OrderStateEnum.Created ||
                        x.State == OrderStateEnum.Accepted && profiles.Contains(x.Customer))
            .ToArrayAsync();
        foreach (var order in orders)
        {
            order.State = OrderStateEnum.Canceled;
        }

        _dbContext.UpdateRange(orders);
        _dbContext.Profiles.RemoveRange(profiles);
        await _dbContext.SaveChangesAsync();
    }
}