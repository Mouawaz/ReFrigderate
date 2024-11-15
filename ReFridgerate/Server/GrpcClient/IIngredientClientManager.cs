using Entities;

namespace GrpcClient;

public interface IIngredientClientManager
{
    Ingredient UpdateIngredient(Ingredient ingredient, int difference);
    IQueryable<Ingredient> GetAllIngredients();
}