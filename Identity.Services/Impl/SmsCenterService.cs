using System.Web;
using Common.Options;
using DataContracts.SmsCenter;
using Identity.Services.Interfaces;
using Services.Interfaces;

namespace Identity.Services.Impl;

public class SmsCenterService:IMessagingService
{
    private readonly SmsCenterConfig _config;
    private readonly IWaygoHttpService _httpService;
    private readonly IHttpClientFactory _clientFactory;

    public SmsCenterService(SmsCenterConfig config,
        IWaygoHttpService httpService,
        IHttpClientFactory clientFactory)
    {
        _config = config;
        _httpService = httpService;
        _clientFactory = clientFactory;
    }

    public async Task<bool> Send(string phone, string message)
    {
        var client = _clientFactory.CreateClient("SmsCenter");
        var smsParams = new SmsCenterParam(_config.Login, _config.Password, phone, message);
        var uriBuilder = new UriBuilder($"{client.BaseAddress}rest/send");
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["login"] = _config.Login;
        query["psw"] = _config.Password;
        query["phones"] = phone;
        query["mes"] = message;
        query["sender"] = _config.Sender;
        uriBuilder.Query = query.ToString() ?? string.Empty;
        var res = await _httpService.SendAsync(client, uriBuilder.Uri.ToString(), smsParams, HttpMethod.Get);
        return res.Data;
    }
}
