using APIContracts.RecipeDtos;

namespace APIContracts.OrderDto;

public class Dish
{
    public RecipeDto Recipe { get; set; }
    public int Quantity { get; set; }
}