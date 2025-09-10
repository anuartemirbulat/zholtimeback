using DataContracts.Messages;
using MassTransit;
using RegionalRides.DAL;
using RegionalRides.DAL.Entities.Entities;

namespace RegionalRides.Api.Consumers;

public class NewProfileConsumer : IConsumer<NewProfileMessage>
{
    private readonly RegionalRidesContext _dbContext;

    public NewProfileConsumer(RegionalRidesContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<NewProfileMessage> context)
    {
        var newProfile = new Profile()
        {
            Guid = context.Message.Guid,
            Nickname = context.Message.Nickname,
            InitialName = context.Message.InitialName,
            PhoneNumber = context.Message.PhoneNumber
        };
        await _dbContext.AddAsync(newProfile);
        await _dbContext.SaveChangesAsync();
    }
}
