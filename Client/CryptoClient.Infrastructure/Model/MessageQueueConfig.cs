namespace CryptoClient.Infrastructure.Model;

public class MessageQueueConfig
{
    public string Server { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Queue { get; set; }
}