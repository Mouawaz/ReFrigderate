using APIContracts.RecipeDtos;

namespace RepositoryContracts;

public interface IRecipeRepository
{
    IQueryable<Recipe> GetAllRecipes();
    Task<Recipe> AddAsync(CreateRecipeDto recipeDto);
    Task<Recipe> UpdateAsync(Recipe recipe);


}