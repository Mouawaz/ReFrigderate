namespace APIContracts.IngridientDtos;

public enum IngredientCategory
{
    MEAT = 0,
    POULTRY = 1,
    VEGETABLE = 2,
    FRUIT = 3,
    LEGUME = 4,
    DAIRY = 5
}

public class IngredientDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float Cost { get; set; }
    public int Amount { get; set; }
    public int DaysUntilBad { get; set; }
    public int StockStatus { get; set; }
    public int ExpirationStatus { get; set; }
    public IngredientCategory Category { get; set; }
}