using APIContracts.IngridientDtos;
using Entities;

namespace RepositoryContracts;
public interface IIngredientRepository
{
    Ingredient UpdateAsync(int id, UpdateIngredientDto userInfo, int difference);
    IQueryable<Ingredient> GetAllIngredients();
}