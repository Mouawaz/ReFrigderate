namespace APIContracts.IngridientDtos;

public class UpdateIngredientDto
{
    public string Name { get; set; }
    public string DateOfExpiration { get; set; }
    public int Amount { get; set; }
    public float Cost { get; set; }
    public int Difference { get; set; }
    public bool Substraction { get; set; }
}