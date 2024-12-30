using ComputerVisionService.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedLib.Contracts;

namespace ComputerVisionService.Consumers;

public class OrderRegisteredConsumer(
    ILogger<OrderRegisteredConsumer> logger,
    IPublishEndpoint publishEndpoint,
    IFaceDetectionServices faceDetectionServices)
    : IConsumer<IOrderRegisteredEvent>
{
    public async Task Consume(ConsumeContext<IOrderRegisteredEvent> context)
    {
        var msg = context.Message;
        msg.FaceData = await faceDetectionServices.ExtractFaces(msg.ImageData);
        await publishEndpoint.Publish<IOrderProcessedEvent>(new
        {
            msg.OrderId,
            msg.FaceData
        });

        logger.LogInformation("the new order contains: {FaceDataCount} faces", msg.FaceData.Count);
    }
}