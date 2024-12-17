namespace APIContracts.OrderDto;

public class OrderDto
{
    public int TableNumber { get; set; }
    public DateTime OrderTime { get; set; }
    public List<Dish> Dishes { get; set; }
}