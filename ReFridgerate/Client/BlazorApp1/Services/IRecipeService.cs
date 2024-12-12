using APIContracts.RecipeDtos;

namespace BlazorApp1.Services;

public interface IRecipeService {
    Task<Recipe> AddRecipeAsync(CreateRecipeDto recipeDto);
    Task<List<RecipeDto>>GetRecipesAsync();
    Task DeleteRecipeAsync(int id);
    Task<RecipeDto> UpdateRecipeAsync(int id, CreateRecipeDto updateRecipe);
}