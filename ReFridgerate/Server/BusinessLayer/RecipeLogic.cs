using APIContracts.RecipeDtos;
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

    public async Task<Recipe> AddAsync(CreateRecipeDto recipeDto)
    {
        if (recipeDto.name.Equals(null) || recipeDto.type.Equals(null) || recipeDto.creatorId.Equals(null) || recipeDto.type.Equals(null))
        {
            throw new ArgumentException("Data cannot be null");
        }
        if (recipeDto.ingredients.Count.Equals(0))
        {
            throw new ArgumentException("Recipe ingredients cannot be empty");
        }
        foreach (RecipeIngredientDto ingredient in recipeDto.ingredients)
        {
            if (ingredient.IngredientId.Equals(null) || ingredient.IngredientId < 1 || ingredient.IngredientName.Equals(null))
            {
                throw new ArgumentException("Invalid recipe ingredient information");
            }
        }
        return await clientManager.AddAsync(recipeDto);
    }

    public async Task<Recipe> UpdateAsync(Recipe recipe)
    {
        if (recipe.Id.Equals(null) || recipe.Name.Equals(null) || recipe.Instruction.Equals(null) || recipe.Ingredients.Count.Equals(0) ||
            recipe.Type.Equals(null) || recipe.CreatorId.Equals(null))
        {
            throw new ArgumentException("Data cannot be null");
        }

        if (recipe.Id < 1 || recipe.Name.All(c =>char.IsDigit(c) || recipe.CreatorId < 1 || recipe.Type.Any(c =>char.IsDigit(c))))
        {
            throw new ArgumentException("Invalid recipe information");
        }
        return await clientManager.UpdateUserAsync(recipe);
    }

    public async Task DeleteAsync(int id)
    {
        if (id.Equals(null) || id < 1)
        {
            throw new ArgumentException("Id cannot be null or negative");
        }

        await clientManager.DeleteRecipeAsync(id);
    }
}