namespace APIContracts.IngridientDtos;

public class UpdateIngredientDto
{

    public string DateOfExpiration { get; set; }
    public int Difference { get; set; }
    public bool Substraction { get; set; }
}