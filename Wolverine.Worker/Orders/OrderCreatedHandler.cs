namespace Wolverine.Worker.Orders;

public class OrderCreatedHandler
{
    public async Task<ProcessOrder> HandleAsync(OrderCreated orderCreated)
    {
        return new ProcessOrder(orderCreated.OrderId, DateTime.Now);
    }
}

public record OrderCreated(Guid OrderId, DateTime CreatedAt);

