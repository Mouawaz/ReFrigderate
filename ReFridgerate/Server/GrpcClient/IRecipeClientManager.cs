using APIContracts.RecipeDtos;

namespace GrpcClient;

public interface IRecipeClientManager
{
    IQueryable<Recipe> GetAllRecipes();
    Task<Recipe> AddAsync(CreateRecipeDto recipeDto);
    Task<Recipe> UpdateUserAsync(Recipe recipe);
    Task DeleteRecipeAsync(int id);
}