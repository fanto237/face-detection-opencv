using MassTransit;
using OrderApi.Models;
using OrderApi.Repository;
using SharedLib.Contracts;

namespace OrderApi.Consumers;

public class OrderSentConsumer : IConsumer<IOrderSentEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderSentConsumer> _logger;

    public OrderSentConsumer(IOrderRepository orderRepository, ILogger<OrderSentConsumer> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<IOrderSentEvent> context)
    {
        var msg = context.Message;
        var order = await _orderRepository.GetById(msg.OrderId);
        order.Status = OrderStatus.Sent;
        await _orderRepository.Save();
        _logger.LogInformation($"the order with the id: {msg.OrderId} has been sent at {msg.SentTime}"); 

    }
}