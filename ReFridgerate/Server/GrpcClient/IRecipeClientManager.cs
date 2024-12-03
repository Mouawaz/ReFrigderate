namespace GrpcClient;

public interface IRecipeClientManager
{
    IQueryable<Recipe> GetAllRecipes();

}