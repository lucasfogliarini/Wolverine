namespace Wolverine.Worker.Orders;

public class OrderProcessedHandler(ILogger<OrderProcessedHandler> logger)
{
    public void Handle(OrderProcessed orderProcessed)
    {
        logger.LogInformation("Order {OrderId} processed at {ProcessedAt}", orderProcessed.OrderId, orderProcessed.ProcessedAt);
    }
}

public record OrderProcessed(Guid OrderId, DateTime ProcessedAt);

