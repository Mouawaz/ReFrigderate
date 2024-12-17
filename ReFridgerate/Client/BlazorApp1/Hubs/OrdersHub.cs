using APIContracts.OrderDto;


namespace BlazorApp1.Hubs;

using Microsoft.AspNetCore.SignalR;

public class OrdersHub : Hub<IOrdersHub>
{
   
    public async Task SendOrder(OrderDto order)
    {
        OrdersSingleton.Instance.add(order);
        //await Clients.All.ReceiveOrder(order);
        await Clients.All.ReceiveOrders(OrdersSingleton.Instance.GetList());

    }
    public async Task CancelOrder(OrderDto order)
    {
        OrdersSingleton.Instance.remove(order);
        await Clients.All.ReceiveCancellation(order);
    }

    public async Task GetOrders()
    {
        await Clients.Caller.ReceiveOrders(OrdersSingleton.Instance.GetList());
    }
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        await Clients.Caller.ReceiveOrders(OrdersSingleton.Instance.GetList());
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