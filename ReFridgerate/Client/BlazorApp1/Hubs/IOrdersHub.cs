using APIContracts.OrderDto;

namespace BlazorApp1.Hubs;

public interface IOrdersHub
{
    Task ReceiveOrder(OrderDto order);
    Task ReceiveCancellation(OrderDto order);
}