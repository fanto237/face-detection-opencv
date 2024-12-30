using ComputerVisionService.Consumers;
using ComputerVisionService.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SharedLib.Contracts;

namespace ComputerVisionService.Tests.Consumers;

public class OrderRegisteredConsumerTests
{
    [Fact]
    public async Task Consume_ShouldPublishOrderProcessedEventWithFaceData()
    {
        // Arrange
        var logger = Substitute.For<ILogger<OrderRegisteredConsumer>>();
        var publishEndpoint = Substitute.For<IPublishEndpoint>();
        var faceDetectionServices = Substitute.For<IFaceDetectionServices>();
        var consumer = new OrderRegisteredConsumer(logger, publishEndpoint, faceDetectionServices);

        var faceData = new List<byte[]> { new byte[] { 1, 2, 3 } };
        faceDetectionServices.ExtractFaces(Arg.Any<byte[]>()).Returns(faceData);

        var context = Substitute.For<ConsumeContext<IOrderRegisteredEvent>>();
        var orderRegisteredEvent = Substitute.For<IOrderRegisteredEvent>();
        orderRegisteredEvent.OrderId.Returns(Guid.NewGuid());
        orderRegisteredEvent.ImageData.Returns(new byte[] { 0xFF, 0xD8, 0xFF });
        context.Message.Returns(orderRegisteredEvent);

        // Act
        await consumer.Consume(context);

        // Assert
        await faceDetectionServices.Received(1).ExtractFaces(context.Message.ImageData);
        await publishEndpoint.Received(1).Publish<IOrderProcessedEvent>(Arg.Is<object>(e =>
            e.GetType().GetProperty("OrderId").GetValue(e).Equals(context.Message.OrderId) &&
            ((List<byte[]>)e.GetType().GetProperty("FaceData").GetValue(e)).Count == faceData.Count
        ));
        // logger.Received(1).LogInformation("the new order contains: {FaceDataCount} faces", faceData.Count);
    }

    [Fact]
    public async Task Consume_NoFacesDetected_ShouldPublishOrderProcessedEventWithEmptyFaceData()
    {
        // Arrange
        var logger = Substitute.For<ILogger<OrderRegisteredConsumer>>();
        var publishEndpoint = Substitute.For<IPublishEndpoint>();
        var faceDetectionServices = Substitute.For<IFaceDetectionServices>();
        var consumer = new OrderRegisteredConsumer(logger, publishEndpoint, faceDetectionServices);

        faceDetectionServices.ExtractFaces(Arg.Any<byte[]>()).Returns(new List<byte[]>());

        var context = Substitute.For<ConsumeContext<IOrderRegisteredEvent>>();
        var orderRegisteredEvent = Substitute.For<IOrderRegisteredEvent>();
        orderRegisteredEvent.OrderId.Returns(Guid.NewGuid());
        orderRegisteredEvent.ImageData.Returns(new byte[] { 0xFF, 0xD8, 0xFF });
        context.Message.Returns(orderRegisteredEvent);


        // Act
        await consumer.Consume(context);

        // Assert
        await faceDetectionServices.Received(1).ExtractFaces(context.Message.ImageData);
        await publishEndpoint.Received(1).Publish<IOrderProcessedEvent>(Arg.Is<object>(e =>
            e.GetType().GetProperty("OrderId").GetValue(e).Equals(context.Message.OrderId) &&
            ((List<byte[]>)e.GetType().GetProperty("FaceData").GetValue(e)).Count == 0
        ));
        // logger.Received(1).LogInformation("the new order contains: {FaceDataCount} faces", 0);
    }
}