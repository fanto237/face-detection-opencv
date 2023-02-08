namespace SharedLib;

public static class RabbitMqConstants
{
    public const string RmqUri = "localhost";
    public const string RmqUsername = "guest";
    public const string RmqPassword = "guest";
    public const string OrderRegisteredEventQueueName = "order.registered.queue";
    public const string OrderProcessedEventQueueName = "order.processed.queue";
    public const string OrderSendEventQueueName = "order.send.queue";
    public const string OrderSentEventQueueName = "order.sent.queue";
}