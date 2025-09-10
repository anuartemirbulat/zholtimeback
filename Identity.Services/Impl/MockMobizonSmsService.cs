using Common.Options;
using Services.Interfaces;

namespace Identity.Services.Impl;

public class MockMobizonSmsService : MobizonSmsService
{
    public MockMobizonSmsService(MobizonOptions config, IWaygoHttpService httpService, IHttpClientFactory clientFactory)
        : base(config, httpService, clientFactory)
    {
    }

    public override async Task<bool> Send(string phone, string message)
    {
        Console.WriteLine($"Типа отправил код подверждения на {phone} code-{message}");
        return true;
    }
}