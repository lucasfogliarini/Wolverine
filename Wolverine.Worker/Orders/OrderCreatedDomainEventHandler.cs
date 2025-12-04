using Wolverine.ErrorHandling;
using Wolverine.Runtime.Handlers;

namespace Wolverine.Worker.Orders;

public class OrderCreatedDomainEventHandler(IMessageBus bus, ILogger<OrderCreatedDomainEventHandler> logger)
{
    public static void Configure(HandlerChain chain, Envelope envelope)
    {
        chain.OnAnyException()
            .RetryTimes(3)
            .Then.MoveToErrorQueue();
    }

    public async Task HandleAsync(OrderCreated orderCreated)
    {
        logger.LogInformation("Processing order: {OrderId} created at {CreatedAt}",  orderCreated.OrderId, orderCreated.CreatedAt);

         await Task.Delay(1000);

        var orderProcessed = new OrderProcessed(orderCreated.OrderId, DateTime.Now);

        logger.LogInformation("Order: {OrderId} processed at {ProcessedAt}", orderProcessed.OrderId, orderProcessed.ProcessedAt);

        await bus.PublishAsync(orderProcessed);
    }
}

public record OrderCreated(Guid OrderId, DateTime CreatedAt);
public record OrderProcessed(Guid OrderId, DateTime ProcessedAt);

