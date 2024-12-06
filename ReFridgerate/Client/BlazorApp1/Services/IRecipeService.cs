using APIContracts.RecipeDtos;

namespace BlazorApp1.Services;

public interface IRecipeService {
    Task<Recipe> AddRecipeAsync(CreateRecipeDto recipeDto);
    Task<List<Recipe>> GetRecipesAsync();
}