using System.Text.Json.Serialization;

namespace Identity.DataContract;

public class MobizonSendMessageRequest
{
    [JsonPropertyName("recipient")]
    public string Recipient { get; set; }
    [JsonPropertyName("text")]
    public string Text { get; set; }

    public MobizonSendMessageRequest(string recipient, string text)
    {
        Recipient = recipient;
        Text = text;
    }
}
