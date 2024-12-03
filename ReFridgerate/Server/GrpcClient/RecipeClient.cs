using APIContracts.RecipeDtos;
using Grpc.Net.Client;

namespace GrpcClient;

public class RecipeClient : IRecipeClientManager
{
    private GrpcChannel channel;
    private RecipeService.RecipeServiceClient recipeService;
    public RecipeClient()
    {
        channel = GrpcChannel.ForAddress("http://localhost:8080");
        recipeService = new RecipeService.RecipeServiceClient(channel);
    }
    public IQueryable<Recipe> GetAllRecipes()
    {

        EmptyRecep empty = new EmptyRecep();
        IQueryable<Recipe> recipes =
            recipeService.GetAllRecipes(empty).Recipes.AsQueryable();
        return recipes.AsQueryable();
    }
}