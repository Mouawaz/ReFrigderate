namespace APIContracts.IngridientDtos;

public class IngredientDto
{
    public int Id {get; set;}
    public string Name {get; set;}
    public float Cost {get; set;}
    public int Amount {get; set;}
    public int DaysUntilBad {get; set;}
    public int StockStatus {get; set;}
}