using APIContracts.OrderDto;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorApp1.Services;

public class OrdersService : IAsyncDisposable
{
    HubConnection hubConnection;
    private readonly NavigationManager navigationManager;
    public event Action<OrderDto> OnOrderReceived;
    public event Action<OrderDto> OnOrderCancelled; 
    public event Action<List<OrderDto>> OnOrdersReceived; 
    public OrdersService(NavigationManager navigationManager)
    {
        this.navigationManager = navigationManager;
    }

    public async Task StartConnection()
    {

        if (hubConnection is not null) return;
        hubConnection = new HubConnectionBuilder()
            .WithUrl(navigationManager.ToAbsoluteUri("/ordershub"), options =>
            {
                options.SkipNegotiation = false;
                options.Transports = HttpTransportType.WebSockets;
            })
            .Build();
        
        hubConnection.On<OrderDto>("ReceiveOrder", (order) => OnOrderReceived?.Invoke(order));
        hubConnection.On<OrderDto>("ReceiveCancellation", (order) => OnOrderCancelled?.Invoke(order));
        hubConnection.On<List<OrderDto>>("ReceiveOrders", (orders) => OnOrdersReceived?.Invoke(orders));
        await hubConnection.StartAsync();
    }

    public async Task SendMessage(OrderDto order)
    {
        if (hubConnection is null) return;
        await hubConnection.SendAsync("SendOrder", order);
    }

    public async Task SendCancellation(OrderDto order)
    {
        if (hubConnection is null) return;
        await hubConnection.SendAsync("CancelOrder", order);
    }

    public async Task RequestOrders()
    {
        if (hubConnection is null) return;
        await hubConnection.SendAsync("GetOrders");
    }
    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
        {
            await hubConnection.DisposeAsync();
        }
    }
}