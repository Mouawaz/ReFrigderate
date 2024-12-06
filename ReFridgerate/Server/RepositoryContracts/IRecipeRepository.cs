using APIContracts.RecipeDtos;

namespace RepositoryContracts;

public interface IRecipeRepository
{
    IQueryable<RecipeDto> GetAllRecipes();
    Task<Recipe> AddAsync(CreateRecipeDto recipeDto);
    Task<Recipe> UpdateAsync(int id, CreateRecipeDto recipe);
    Task DeleteAsync(int id);


}