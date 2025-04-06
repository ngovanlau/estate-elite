namespace EventBus.RabbitMQ.Settings;

public class RabbitMQSetting
{
    public string HostName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = string.Empty;
    public int Port { get; set; } = 5672;
    public int RetryCount { get; set; } = 5;
}
