using APIContracts.IngridientDtos;

namespace BlazorApp1.Services;

public interface IIngredientService
{
    Task<IngredientDto> AddIngredientAsync(IngredientDto request);
    Task<List<IngredientDto>> GetIngredientsAsync();
}