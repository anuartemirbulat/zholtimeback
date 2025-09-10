namespace DataContracts.Messages;

public class NewProfileMessage
{
    public Guid Guid { get; set; }
    public string InitialName { get; set; }
    public string Nickname { get; set; }
    public string PhoneNumber { get; set; }

    public NewProfileMessage(Guid guid, string initialName, string nickname, string phoneNumber)
    {
        Guid = guid;
        InitialName = initialName;
        Nickname = nickname;
        PhoneNumber = phoneNumber;
    }
}
