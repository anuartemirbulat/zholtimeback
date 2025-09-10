using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Core;

public class AuthOptions
{
    public const string ISSUER = "AgricultureMinistry";
    public const string AUDIENCE = "https://vetlab.gov.kz";
    const string KEY = "w$mw=Pgh6k23klsEs*GJNM*Uc=yPKg?2N";
    public static readonly TimeSpan AccessTokenLifeTime = TimeSpan.FromDays(60);

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }

    public static bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken,
        TokenValidationParameters validationParameters)
    {
        if (expires != null)
            return DateTime.UtcNow < expires;
        return false;
    }
}
