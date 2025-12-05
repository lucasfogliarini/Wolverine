namespace Wolverine.Worker.Orders;

public class ProcessOrderHandler(ILogger<ProcessOrderHandler> logger)
{
    public async Task<OrderProcessed> HandleAsync(ProcessOrder processOrder)
    {
        logger.LogInformation("Processing order: {OrderId}", processOrder.OrderId);

         await Task.Delay(1000);

        var orderProcessed = new OrderProcessed(processOrder.OrderId, DateTime.Now);

        logger.LogInformation("Order: {OrderId} processed at {ProcessedAt}", orderProcessed.OrderId, orderProcessed.ProcessedAt);

        return orderProcessed;
    }
}

public record ProcessOrder(Guid OrderId, DateTime ProcessedAt);



