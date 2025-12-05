using Wolverine.ErrorHandling;
using Wolverine.Runtime.Handlers;

namespace Wolverine.Worker.Orders;

public class OrderCreatedDomainEventHandler(IMessageBus bus, ILogger<OrderCreatedDomainEventHandler> logger)
{
    public static void Configure(HandlerChain chain, Envelope envelope)
    {
        chain.OnAnyException()
            .RetryTimes(3)
            .Then.Discard().And(async (_, context, _) =>
            {
                await context.PublishAsync(context.Envelope.Message);
            });

    }

    public async Task HandleAsync(OrderCreated orderCreated)
    {
        //throw new();

        await bus.PublishAsync(new ProcessOrderCommand(orderCreated.OrderId, DateTime.Now));
    }
}

public record OrderCreated(Guid OrderId, DateTime CreatedAt);

