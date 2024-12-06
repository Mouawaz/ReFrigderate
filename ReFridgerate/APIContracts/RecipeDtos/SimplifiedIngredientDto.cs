namespace APIContracts.RecipeDtos;

public class SimplifiedIngredientDto
{
    public int ingredientId { get; set; }
    public string ingredientName { get; set; }
    public int ingredientQuantity { get; set; }
    public float ingredientCost  { get; set; }
}