namespace SharedLib;

public static class RabbitMqConstants
{
    public const string RmqUri = "localhost";
    public const string RmqUsername = "guest";
    public const string RmqPassword = "guest";
    public const string OrderRegisteredEvent = "order.registered.event";
    public const string OrderProcessedEvent = "order.processed.event";
    public const string OrderSentEvent = "order.sent.event";
}