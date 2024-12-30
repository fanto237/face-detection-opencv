using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using OrderApi.Consumers;
using OrderApi.Models;
using OrderApi.Repository;
using SharedLib.Contracts;

namespace OrderApi.Tests.Consumers;

public class OrderProcessedConsumerTests
{
    private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
    private readonly IFaceRepository _faceRepository = Substitute.For<IFaceRepository>();
    private readonly IPublishEndpoint _publishEndpoint = Substitute.For<IPublishEndpoint>();
    private readonly ILogger<OrderProcessedConsumer> _logger = Substitute.For<ILogger<OrderProcessedConsumer>>();

    [Fact]
    public async Task Consume_CreatesFacesInDatabase()
    {
        // Arrange
        var consumer = new OrderProcessedConsumer(_orderRepository, _logger, _faceRepository, _publishEndpoint);
        var context = Substitute.For<ConsumeContext<IOrderProcessedEvent>>();
        var faceData = new List<byte[]> { new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6 } };
        var processedOrder = Substitute.For<IOrderProcessedEvent>();
        processedOrder.OrderId.Returns(Guid.NewGuid());
        processedOrder.FaceData.Returns(faceData);
        context.Message.Returns(processedOrder);
        _orderRepository.GetById(Arg.Any<Guid>()).Returns(new Order());

        // Act
        await consumer.Consume(context);

        // Assert
        await _faceRepository.Received(faceData.Count).Create(Arg.Any<Face>());
    }

    [Fact]
    public async Task Consume_UpdatesOrderStatusToProcessed()
    {
        // Arrange
        var consumer = new OrderProcessedConsumer(_orderRepository, _logger, _faceRepository, _publishEndpoint);
        var context = Substitute.For<ConsumeContext<IOrderProcessedEvent>>();
        var order = new Order { OrderId = Guid.NewGuid(), Status = OrderStatus.Registered };
        var processedOrder = Substitute.For<IOrderProcessedEvent>();
        processedOrder.OrderId.Returns(order.OrderId);
        processedOrder.FaceData.Returns(new List<byte[]>());
        context.Message.Returns(processedOrder);
        _orderRepository.GetById(order.OrderId).Returns(order);

        // Act
        await consumer.Consume(context);

        // Assert
        Assert.Equal(OrderStatus.Processed, order.Status);
        await _orderRepository.Received(1).Save();
    }

    [Fact]
    public async Task Consume_PublishesOrderSendEvent()
    {
        // Arrange
        var consumer = new OrderProcessedConsumer(_orderRepository, _logger, _faceRepository, _publishEndpoint);
        var context = Substitute.For<ConsumeContext<IOrderProcessedEvent>>();
        var faceData = new List<byte[]> { new byte[] { 1, 2, 3 } };
        var order = new Order { OrderId = Guid.NewGuid(), Username = "user", Email = "user@example.com", ImageName = "image.jpg" };
        var processedOrder = Substitute.For<IOrderProcessedEvent>();
        processedOrder.OrderId.Returns(order.OrderId);
        processedOrder.FaceData.Returns(faceData);
        context.Message.Returns(processedOrder);
        _orderRepository.GetById(order.OrderId).Returns(order);

        // Act
        await consumer.Consume(context);

        // Assert
        await _publishEndpoint.Received(1).Publish<IOrderSendEvent>(Arg.Any<object>());
    }
}