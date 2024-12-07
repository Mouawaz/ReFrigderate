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
        List<RecipeDto> recipes =
            recipeService.GetAllRecipes(empty).Recipes.AsQueryable().Select(r =>
                new RecipeDto()
                {
                    id = r.Id,
                    name = r.Name,
                    creatorId = r.CreatorId,
                    type = r.Type,
                    instruction = r.Instructions,
                    modifcationsAllowed = r.ModificationsAllowed,
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

    public async Task<RecipeDto> AddAsync(CreateRecipeDto recipeDto)
    {
         CreateRecipeRequest request = new()
         {
             Name = recipeDto.name,
             Instructions = recipeDto.instructions,
             CreatorId = recipeDto.creatorId,
             Type = recipeDto.type,
             ModificationsAllowed = recipeDto.modificationsAllowed,
             Ingredients = { recipeDto.ingredients.AsQueryable().Select(ingredient => new SimplifiedIngredient()
             {
                 IngredientId = ingredient.IngredientId,
                 IngredientName = ingredient.IngredientName,
                 Quantity = ingredient.Quantity
             }) }
         };
         Recipe recipe = await recipeService.CreateRecipeAsync(request);
         RecipeDto dto = new()
         {
             id = recipe.Id,
             name = recipe.Name,
             creatorId = recipe.CreatorId,
             type = recipe.Type,
             instruction = recipe.Instructions,
             modifcationsAllowed = recipe.ModificationsAllowed,
             ingredients = recipe.Ingredients.AsQueryable().Select(ingredient =>
                 new SimplifiedIngredientDto()
                 {
                     ingredientId = ingredient.IngredientId,
                     ingredientName = ingredient.IngredientName,
                     ingredientCost = ingredient.Cost,
                     ingredientQuantity = ingredient.Quantity
                 }).ToList()
         };
         return dto;
    }

    public async Task<RecipeDto> UpdateRecipeAsync(int id,
        CreateRecipeDto recipeDto)
    {
        CreateRecipeRequest request = new()
         {
             UpdateRecipeId = id,
             Name = recipeDto.name,
             Instructions = recipeDto.instructions,
             Type = recipeDto.type,
             CreatorId = recipeDto.creatorId,
             ModificationsAllowed = recipeDto.modificationsAllowed,

             Ingredients = { recipeDto.ingredients.AsQueryable().Select(i =>
                     new SimplifiedIngredient()
                     {
                         IngredientId = i.IngredientId,
                         IngredientName = i.IngredientName,
                         Quantity = i.Quantity
                     }).ToList()
             }
         };

         Recipe recipe = await recipeService.UpdateRecipeAsync(request);
         RecipeDto dto = new()
         {
             id = recipe.Id,
             name = recipe.Name,
             creatorId = recipe.CreatorId,
             type = recipe.Type,
             instruction = recipe.Instructions,
             modifcationsAllowed = recipe.ModificationsAllowed,
             ingredients = recipe.Ingredients.AsQueryable().Select(ingredient =>
                 new SimplifiedIngredientDto()
                 {
                     ingredientId = ingredient.IngredientId,
                     ingredientName = ingredient.IngredientName,
                     ingredientCost = ingredient.Cost,
                     ingredientQuantity = ingredient.Quantity
                 }).ToList()
         };
         return dto;
     }

     public async Task DeleteRecipeAsync(int id)
     {
         DeleteRecipeRequest request = new() { Id = id };
         DeleteResponse response = await recipeService.DeleteRecipeAsync(request);
         if (!response.IsSucces)
         {
             throw new Exception("Failed to delete recipe");
         }
         return;
     }
}