using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

public static class OrderOrchestrator
{
    [FunctionName("OrderOrchestrator")]
    public static async Task RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        var order = context.GetInput<Order>();

        // Update inventory
        await context.CallActivityAsync("UpdateInventory", order);

        // Process payment
        await context.CallActivityAsync("ProcessPayment", order);

        // Send order confirmation
        await context.CallActivityAsync("SendOrderConfirmation", order);

        // Send notifications
        await context.CallActivityAsync("SendNotification", new Notification { Message = "Order processed" });

        order.orderStatus = "Processed";
        await context.CallActivityAsync("UpdateOrderStatus", order);
    }
}

public class Order
{
    public int orderID { get; set; }
    public int userID { get; set; }
    public int productID { get; set; }
    public string deliveryCountry { get; set; }
    public string shippingMethod { get; set; }
    public string shippingAddress { get; set; }
    public string phoneNumber { get; set; }
    public string orderStatus { get; set; }
}

public class Notification
{
    public string Message { get; set; }
}
