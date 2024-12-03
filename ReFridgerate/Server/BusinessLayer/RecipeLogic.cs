using GrpcClient;
using RepositoryContracts;

namespace BusinessLayer;

public class RecipeLogic : IRecipeRepository
{
    
    public readonly IRecipeClientManager clientManager;

    public RecipeLogic(IRecipeClientManager clientManager)
    {
        this.clientManager = clientManager;
    }
    public IQueryable<Recipe> GetAllRecipes()
    {
        return clientManager.GetAllRecipes();
    }
}