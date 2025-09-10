namespace Identity.Services.Interfaces;

public interface IMessagingService
{
    Task<bool> Send(string phone, string message);
}
