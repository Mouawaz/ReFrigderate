namespace APIContracts.IngridientDtos;

public class CreateIngredientDto {
    public string name { get; set; }
    public string category { get; set; }
    public float cost { get; set; }
}