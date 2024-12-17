using APIContracts.OrderDto;

namespace BlazorApp1;

public sealed class OrdersSingleton
{
    private List<OrderDto> orders;
    private static readonly Lazy<OrdersSingleton> lazy =
        new Lazy<OrdersSingleton>(() => new OrdersSingleton());  
    public static OrdersSingleton Instance { get { return lazy.Value; } }
    public OrdersSingleton() {
        orders = new();
    }
    
    public void add(OrderDto order) {
        orders.Add(order);
    }
    public List<OrderDto> GetList() {
        return orders;
    }

    public void remove(OrderDto givenOrder)
    {
        foreach (var order in orders)
        {
            if (order.OrderTime == givenOrder.OrderTime || order.TableNumber == givenOrder.TableNumber)
            {
                orders.Remove(order);
                return;
            }
        }
    }
}