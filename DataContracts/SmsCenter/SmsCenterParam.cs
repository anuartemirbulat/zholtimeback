namespace DataContracts.SmsCenter;

public class SmsCenterParam
{
    public string Login { get; set; }
    public string Psw { get; set; }
    public string Phones { get; set; }
    public string Mes { get; set; }
    public int Fmt { get; set; }

    public SmsCenterParam(string login, string password, string phones, string mes)
    {
        Login = login;
        Psw = password;
        Phones = phones;
        Mes = mes;
        Fmt = 3;
    }
}
