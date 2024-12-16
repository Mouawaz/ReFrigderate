using APIContracts.OrderDto;


namespace BlazorApp1.Hubs;

using Microsoft.AspNetCore.SignalR;

public class OrdersHub : Hub<IOrdersHub>
{
    public async Task SendOrder(OrderDto order)
    {
        await Clients.All.ReceiveOrder(order);
    }

    public async Task CancelOrder(OrderDto order)
    {
        await Clients.All.ReceiveCancellation(order);
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
    /*public async Task SendOrder(Order order)
    {
        Console.WriteLine($"Sending order {order}");
        await Clients.All.SendAsync("ReceiveOrder", order);
    }*/
}