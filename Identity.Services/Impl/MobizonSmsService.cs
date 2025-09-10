using System.Web;
using Common.Options;
using DataContracts.SmsCenter;
using Identity.DataContract;
using Identity.Services.Interfaces;
using Services.Interfaces;

namespace Identity.Services.Impl;

public class MobizonSmsService : IMessagingService
{
    private readonly MobizonOptions _config;
    private readonly IWaygoHttpService _httpService;
    private readonly IHttpClientFactory _clientFactory;

    public MobizonSmsService(MobizonOptions config,
        IWaygoHttpService httpService,
        IHttpClientFactory clientFactory)
    {
        _config = config;
        _httpService = httpService;
        _clientFactory = clientFactory;
    }

    public virtual async Task<bool> Send(string phone, string message)
    {
        var client = _clientFactory.CreateClient("Mobizon");
        var smsParams = new MobizonSendMessageRequest(phone, message);
        var uriBuilder = new UriBuilder($"{client.BaseAddress}/message/SendSmsMessage");
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["apiKey"] = _config.ApiKey;
        uriBuilder.Query = query.ToString() ?? string.Empty;
        var res = await _httpService.SendAsync(client, uriBuilder.Uri.ToString(), smsParams, HttpMethod.Post);
        Console.WriteLine($"Реально отправил код подверждения на {phone} code-{message}");
        return res.Data;
    }
}