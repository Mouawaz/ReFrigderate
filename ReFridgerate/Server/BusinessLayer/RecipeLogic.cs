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
    public IQueryable<RecipeDto> GetAllRecipes()
    {
        return clientManager.GetAllRecipes();
    }

    public async Task<RecipeDto> AddAsync(CreateRecipeDto recipeDto)
    {
        if (recipeDto.name == null || recipeDto.type == null)
        {
            throw new ArgumentException("Data cannot be null");
        }
        if (recipeDto.ingredients.Count.Equals(0))
        {
            throw new ArgumentException("Recipe ingredients cannot be empty");
        }
        foreach (RecipeIngredientDto ingredient in recipeDto.ingredients)
        {
            if (ingredient.IngredientId < 1 || ingredient.IngredientName == null)
            {
                throw new ArgumentException("Invalid recipe ingredient information");
            }
        }
        return await clientManager.AddAsync(recipeDto);
    }

    public async Task<RecipeDto> UpdateAsync(int id, CreateRecipeDto recipe)
    {
        if (recipe.name == null || recipe.ingredients.Count.Equals(0) ||
            recipe.type == null)
        {
            throw new ArgumentException("Data cannot be null");
        }

        if (id < 1 || recipe.creatorId < 1 || recipe.type.Any(c =>char.IsDigit(c)))
        {
            throw new ArgumentException("Invalid recipe information");
        }
        foreach (RecipeIngredientDto ingredient in recipe.ingredients)
        {
            if (ingredient.IngredientId < 1 || ingredient.IngredientName == null)
            {
                throw new ArgumentException("Invalid recipe ingredient information");
            }
        }
        return await clientManager.UpdateRecipeAsync(id, recipe);
    }

    public async Task DeleteAsync(int id)
    {
        if (id < 1)
        {
            throw new ArgumentException("Id cannot be null or negative");
        }

        await clientManager.DeleteRecipeAsync(id);
    }
}