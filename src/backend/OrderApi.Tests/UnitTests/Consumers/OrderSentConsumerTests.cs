using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using OrderApi.Consumers;
using OrderApi.Models;
using OrderApi.Repository;
using SharedLib.Contracts;

namespace OrderApi.Tests.Consumers;

public class OrderSentConsumerTests
{
    private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
    private readonly ILogger<OrderSentConsumer> _logger = Substitute.For<ILogger<OrderSentConsumer>>();

    [Fact]
    public async Task Consume_UpdatesOrderStatusToSent()
    {
        // Arrange
        var consumer = new OrderSentConsumer(_orderRepository, _logger);
        var context = Substitute.For<ConsumeContext<IOrderSentEvent>>();
        var order = new Order { OrderId = Guid.NewGuid(), Status = OrderStatus.Processed };
        // IOrderSentEvent { OrderId = order.OrderId, SentTime = DateTime.UtcNow }
        var sendOrder = Substitute.For<IOrderSentEvent>();
        sendOrder.OrderId.Returns(order.OrderId);
        sendOrder.SentTime.Returns(DateTime.UtcNow);
        context.Message.Returns(sendOrder);
        _orderRepository.GetById(order.OrderId).Returns(order);

        // Act
        await consumer.Consume(context);

        // Assert
        Assert.Equal(OrderStatus.Sent, order.Status);
        await _orderRepository.Received(1).Save();
    }

    // [Fact]
    // public async Task Consume_LogsSentTime()
    // {
    //     // Arrange
    //     var consumer = new OrderSentConsumer(_orderRepository, _logger);
    //     var context = Substitute.For<ConsumeContext<IOrderSentEvent>>();
    //     var sentTime = DateTime.UtcNow;
    //     var sendOrder = Substitute.For<IOrderSentEvent>();
    //     sendOrder.OrderId.Returns(Guid.NewGuid());
    //     sendOrder.SentTime.Returns(sentTime);
    //     context.Message.Returns(sendOrder);
    //
    //     // Act
    //     await consumer.Consume(context);
    //
    //     // Assert
    //     // _logger.Received(1).LogInformation("the order with the id: {OrderId} has been sent at {SentTime}", context.Message.OrderId, sentTime);
    //     // _logger.Received(1).LogInformation("the order with the id: {OrderId} has been sent at {SentTime}", Arg.Any<object[]>());
    //     // _logger.DidNotReceive().LogInformation(Arg.Any<string>(), Arg.Any<object[]>());
    //
    //
    //
    // }
}