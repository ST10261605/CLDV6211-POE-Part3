using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

public static class ActivityFunctions
{
    [FunctionName("UpdateInventory")]
    public static async Task UpdateInventory([ActivityTrigger] Order order, ILogger log)
    {
        log.LogInformation($"Updating inventory for order {order.orderID}.");
        // Logic to update the inventory
        await Task.CompletedTask;
    }

    [FunctionName("ProcessPayment")]
    public static async Task ProcessPayment([ActivityTrigger] Order order, ILogger log)
    {
        log.LogInformation($"Processing payment for order {order.orderID}.");
        // Logic to process payment
        await Task.CompletedTask;
    }

    [FunctionName("SendOrderConfirmation")]
    public static async Task SendOrderConfirmation([ActivityTrigger] Order order, ILogger log)
    {
        log.LogInformation($"Sending order confirmation for order {order.orderID}.");
        // Logic to send order confirmation
        await Task.CompletedTask;
    }

    [FunctionName("SendNotification")]
    public static async Task SendNotification([ActivityTrigger] Notification notification, ILogger log)
    {
        log.LogInformation($"Sending notification: {notification.Message}");
        // Logic to send notification
        await Task.CompletedTask;
    }

    [FunctionName("UpdateOrderStatus")]
    public static async Task UpdateOrderStatus([ActivityTrigger] Order order, ILogger log)
    {
        log.LogInformation($"Updating order status for order {order.orderID} to {order.orderStatus}.");
        // Logic to update the order status in the database
        await Task.CompletedTask;
    }
}
