namespace APIContracts.RecipeDtos;

public class RecipeIngredientDto
{
   public int IngredientId { get; set; }
   public string IngredientName { get; set; }
   public float Cost  { get; set; }
   public int Quantity { get; set; }
}