using APIContracts.RecipeDtos;

namespace GrpcClient;

public interface IRecipeClientManager
{
    IQueryable<RecipeDto> GetAllRecipes();
    Task<RecipeDto> AddAsync(CreateRecipeDto recipeDto);
    Task<RecipeDto> UpdateRecipeAsync(int id, CreateRecipeDto recipe);
    Task DeleteRecipeAsync(int id);
}