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

        EmptyRecep empty = new ();
        IQueryable<Recipe> recipes =
            recipeService.GetAllRecipes(empty).Recipes.AsQueryable();
        return recipes.AsQueryable();
    }

    public async Task<Recipe> AddAsync(CreateRecipeDto recipeDto)
    {
        RecipeCreationRequest request = new()
        {
            Name = recipeDto.name,
            Instruction = recipeDto.instructions,
            Ingredients = { recipeDto.ingredients.AsQueryable().Select(ingredient => new RecipeIngredientBasic()
            {
                IngredientId = ingredient.IngredientId,
                IngredientName = ingredient.IngredientName,
                Quantity = ingredient.Quantity
            }) }
        };
        RecipeResponse response = await recipeService.CreateRecipeAsync(request);
        if (!response.Success)
        {
            throw new Exception(response.Message);
        }
        return response.Recipe;
    }

    public async Task<Recipe> UpdateUserAsync(Recipe recipe)
    {
        RecipeResponse response = await recipeService.UpdateRecipeAsync(recipe);
        if (!response.Success)
        {
            throw new Exception(response.Message);
        }
        return response.Recipe;
    }

    public async Task DeleteRecipeAsync(int id)
    {
        DeleteRecipeRequest request = new() { Id = id };
        RecipeResponse response = await recipeService.DeleteRecipeAsync(request);
        if (!response.Success)
        {
            throw new Exception(response.Message);
        }
        return;
    }
}