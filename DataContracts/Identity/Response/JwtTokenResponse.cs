namespace DataContracts.Identity.Response;

public class JwtTokenResponse
{
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpiration { get; set; }
    public JwtTokenResponse(string accessToken)
    {
        AccessToken = accessToken;
    }
}
