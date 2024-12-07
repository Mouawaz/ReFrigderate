using APIContracts.RecipeDtos;

namespace RepositoryContracts;

public interface IRecipeRepository
{
    IQueryable<RecipeDto> GetAllRecipes();
    Task<RecipeDto> AddAsync(CreateRecipeDto recipeDto);
    Task<RecipeDto> UpdateAsync(int id, CreateRecipeDto recipe);
    Task DeleteAsync(int id);


}