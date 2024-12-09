namespace APIContracts.IngridientDtos;

public class ThresholdDto
{
    public int IndredientId { get; set; }
    public int yellowAmount { get; set; }
    public int redAmount { get; set; }
    public int yellowDaysUntil { get; set; }
    public int redDaysUntil { get; set; }
}