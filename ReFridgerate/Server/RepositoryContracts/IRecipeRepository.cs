namespace RepositoryContracts;

public interface IRecipeRepository
{
    IQueryable<Recipe> GetAllRecipes();

}