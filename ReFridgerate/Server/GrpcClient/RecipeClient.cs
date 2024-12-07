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

    public IQueryable<RecipeDto> GetAllRecipes()
    {

        EmptyRecep empty = new();
        IQueryable<Recipe> recipess =
            recipeService.GetAllRecipes(empty).Recipes.AsQueryable();
        List<RecipeDto> recipes =
            recipeService.GetAllRecipes(empty).Recipes.AsQueryable().Select(r =>
                new RecipeDto()
                {
                    id = r.Id,
                    name = r.Name,
                    creatorId = r.CreatorId,
                    type = r.Type,
                    instruction = r.Instructions,
                    ingredients = r.Ingredients.AsQueryable().Select(i =>
                        new SimplifiedIngredientDto()
                        {
                            ingredientId = i.IngredientId,
                            ingredientName = i.IngredientName,
                            ingredientQuantity = i.Quantity,
                            ingredientCost = i.Cost,
                        }).ToList()
                }).ToList();

        return recipes.AsQueryable();
    }

    public async Task<Recipe> AddAsync(CreateRecipeDto recipeDto)
    {
        /* CreateRecipeRequest request = new()
         {
             Name = recipeDto.name,
             Instructions = recipeDto.instructions,
             CreatorId = recipeDto.creatorId,
             Type = recipeDto.type,
             Ingredients = { recipeDto.ingredients.AsQueryable().Select(ingredient => new SimplifiedIngredient()
             {
                 IngredientId = ingredient.IngredientId,
                 IngredientName = ingredient.IngredientName,
                 IngredientQuantity = ingredient.Quantity
             }) }
         };
         RecipeResponse response = await recipeService.CreateRecipeAsync(request);
         if (!response.Success)
         {
             throw new Exception(response.Message);
         }
         return response.Recipe;*/
        throw new NotImplementedException();
    }

    public async Task<Recipe> UpdateRecipeAsync(int id,
        CreateRecipeDto recipeDto)
    {
        /* Recipe recipe = new()
         {
             Id = id,
             Name = recipeDto.name,
             Instruction = recipeDto.instructions,
             Type = recipeDto.type,
             CreatorId = recipeDto.creatorId,
             Ingredients =
             {
                 recipeDto.ingredients.AsQueryable().Select(i =>
                     new SimplifiedIngredient()
                     {
                         IngredientId = i.IngredientId,
                         IngredientName = i.IngredientName,
                         IngredientCost = i.Quantity,
                         IngredientQuantity = i.Quantity
                     }).ToList()
             }
         };

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
     }*/
        throw new NotImplementedException();
    }

    public Task DeleteRecipeAsync(int id)
    {
        throw new NotImplementedException();
    }
}