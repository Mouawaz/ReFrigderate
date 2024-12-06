using APIContracts.RecipeDtos;

namespace GrpcClient;

public interface IRecipeClientManager
{
    IQueryable<Recipe> GetAllRecipes();
    Task<Recipe> AddAsync(CreateRecipeDto recipeDto);
    Task<Recipe> UpdateRecipeAsync(int id, CreateRecipeDto recipe);
    Task DeleteRecipeAsync(int id);
}