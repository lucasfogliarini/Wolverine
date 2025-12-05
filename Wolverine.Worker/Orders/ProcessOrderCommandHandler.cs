using Wolverine.ErrorHandling;
using Wolverine.Runtime.Handlers;

namespace Wolverine.Worker.Orders;

public class ProcessOrderCommandHandler(IMessageBus bus, ILogger<ProcessOrderCommandHandler> logger)
{
    public async Task HandleAsync(ProcessOrderCommand processOrderCommand)
    {
        logger.LogInformation("Processing order: {OrderId}", processOrderCommand.OrderId);

         await Task.Delay(1000);

        var orderProcessedDomainEvent = new OrderProcessedDomainEvent(processOrderCommand.OrderId, DateTime.Now);

        logger.LogInformation("Order: {OrderId} processed at {ProcessedAt}", orderProcessedDomainEvent.OrderId, orderProcessedDomainEvent.ProcessedAt);

        await bus.PublishAsync(orderProcessedDomainEvent);
    }
}

public record ProcessOrderCommand(Guid OrderId, DateTime ProcessedAt);
public record OrderProcessedDomainEvent(Guid OrderId, DateTime ProcessedAt);

