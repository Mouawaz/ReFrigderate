using APIContracts.IngridientDtos;
using Entities;

namespace RepositoryContracts;
public interface IIngredientRepository
{
    Task<IngredientDto> UpdateAsync(int id, UpdateIngredientDto userInfo, int difference);
    IQueryable<IngredientDto> GetAllIngredients();
}