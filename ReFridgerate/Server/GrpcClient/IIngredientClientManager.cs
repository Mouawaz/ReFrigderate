using APIContracts.IngridientDtos;
using Entities;

namespace GrpcClient;

public interface IIngredientClientManager
{
    IngredientDto UpdateIngredient(IngredientDto IngredientDto, int difference);
    IQueryable<IngredientDto> GetAllIngredients();
    Task<IngredientDto> CreateIngredient(CreateIngredientDto dto);
    Task<ThresholdDto> GetTresholdAsync(int id);
    Task<bool> UpdateTresholdsAsync(ThresholdDto thresholdDto);
}