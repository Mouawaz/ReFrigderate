namespace APIContracts.RecipeDtos;

public class RecipeDto
{
    public int id { get; set; }
    public string name { get; set; }
    public string instruction { get; set; }
    public string type { get; set; }
    public int creatorId { get; set; }
    public bool modifcationsAllowed { get; set; }
    public List<SimplifiedIngredientDto> ingredients { get; set; }
}