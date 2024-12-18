using APIContracts.IngridientDtos;

namespace BlazorApp1.Services;

public interface IIngredientService
{
    Task<IngredientDto> AddIngredientAsync(CreateIngredientDto request);
    Task<IngredientDto> UpdateIngredientAsync(int id, UpdateIngredientDto updateIngredient);
    Task<List<IngredientDto>> GetIngredientsAsync();
    Task<ThresholdDto> GetThresholds(int id);
    Task UpdateThreshold(int id, ThresholdDto updateThreshold);
}