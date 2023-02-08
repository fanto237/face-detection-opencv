using MassTransit;
using SharedLib.Contracts;

namespace OrderApi.Consumers;

public class OrderSentConsumer : IConsumer<IOrderSentEvent>
{
    public Task Consume(ConsumeContext<IOrderSentEvent> context)
    {
        throw new NotImplementedException();
    }
}