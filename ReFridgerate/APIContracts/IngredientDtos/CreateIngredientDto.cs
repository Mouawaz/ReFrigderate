namespace APIContracts.IngridientDtos;

public class CreateIngredientDto {
    public string Name { get; set; }
    public string Category { get; set; }
    public float Cost { get; set; }
}