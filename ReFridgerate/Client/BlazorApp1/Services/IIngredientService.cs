using APIContracts.IngridientDtos;

namespace BlazorApp1.Services;

public interface IIngredientService
{
    Task<IngredientDto> AddIngredientAsync(IngredientDto request);
    Task<IngredientDto> UpdateIngredientAsync(int id, UpdateIngredientDto updateIngredient);
    Task<List<IngredientDto>> GetIngredientsAsync();
}