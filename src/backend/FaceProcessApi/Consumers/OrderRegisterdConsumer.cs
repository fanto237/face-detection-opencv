using MassTransit;
using SharedLib.Contracts;

namespace FaceProcessApi.Consumers;

public class OrderRegisterdConsumer : IConsumer<IOrderRegisteredEvent>
{
    private readonly ILogger<OrderRegisterdConsumer> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IFaceHandler _faceHandler;

    public OrderRegisterdConsumer(ILogger<OrderRegisterdConsumer> logger, IPublishEndpoint publishEndpoint, IFaceHandler faceHandler)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _faceHandler = faceHandler;
    }
    public Task Consume(ConsumeContext<IOrderRegisteredEvent> context)
    {
        var msg = context.Message;
        // order = _faceHandler.ExtractFaces(order);
        // _publishEndpoint.Publish(order);

        _logger.LogInformation($"the new order id is: {msg.OrderId}");
        
        return Task.CompletedTask;
    }
}