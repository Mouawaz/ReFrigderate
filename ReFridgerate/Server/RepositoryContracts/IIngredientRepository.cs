using APIContracts.IngridientDtos;


namespace RepositoryContracts;
public interface IIngredientRepository
{
    Task<IngredientDto> UpdateAsync(int id, UpdateIngredientDto userInfo);
    IQueryable<IngredientDto> GetAllIngredients();
    Task<ThresholdDto> GetSingleTresholdAsync(int id);
    Task<bool> UpdateTresholdsAsync(int id, ThresholdDto thresholdDto);
}