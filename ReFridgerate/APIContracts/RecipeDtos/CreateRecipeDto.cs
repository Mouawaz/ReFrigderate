namespace APIContracts.RecipeDtos;

public class CreateRecipeDto
{
   public string name { get; set; }
   public string instructions { get; set; }
   public string type { get; set; }
   public int creatorId { get; set; }
   public bool modificationsAllowed { get; set; }
   public List<RecipeIngredientDto> ingredients { get; set; }
}