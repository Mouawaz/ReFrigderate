using APIContracts.IngridientDtos;
using Entities;
using Google.Protobuf;
using Grpc.Net.Client;
using IngredientCategory = APIContracts.IngridientDtos.IngredientCategory;

namespace GrpcClient;

public class IngredientClient : IIngredientClientManager
{
    private GrpcChannel channel;
    private IngredientService.IngredientServiceClient ingredientService;

    public IngredientClient()
    {
        channel = GrpcChannel.ForAddress("http://localhost:8080");
        ingredientService = new IngredientService.IngredientServiceClient(channel);
    }
    
    
    public IngredientDto UpdateIngredient(IngredientDto ingredientDto, int difference)
    {
        UpdateIngredientRequest request = new()
        {
            Id = ingredientDto.Id,
            DaysUntilBad = ingredientDto.DaysUntilBad,
            Difference = difference
        };
        Ingredient ingredient =  ingredientService.UpdateIngredient(request);
        IngredientDto responseDto = new()
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            Cost = ingredient.Cost,
            Amount = ingredient.Amount,
            DaysUntilBad = ingredient.DaysUntilBad,
            StockStatus = ingredient.StockStatus,
            Category = (IngredientCategory)ingredient.Category
            ExpirationStatus = ingredient.ExpirationStatus
        };
        return responseDto;
    }

    public IQueryable<IngredientDto> GetAllIngredients()
    {
        Entities.Empty  empty = new Entities.Empty();
        IQueryable<Ingredient> ingredients = ingredientService.GetAllIngredients(empty).Messages.AsQueryable();
        List<IngredientDto> ingredientDtos = new();
        foreach (Ingredient ingredient in ingredients)
        {
            IngredientDto dto = new()
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Cost = ingredient.Cost,
                Amount = ingredient.Amount,
                DaysUntilBad = ingredient.DaysUntilBad,
                StockStatus = ingredient.StockStatus,
                ExpirationStatus = ingredient.ExpirationStatus,
                Category = (IngredientCategory)ingredient.Category  
            };
            ingredientDtos.Add(dto);
        }
        
        return ingredientDtos.AsQueryable();
    }

    public async Task<ThresholdDto> GetTresholdAsync(int id)
    {
        IdRequest request = new IdRequest { Id = id };
        UpdateWarningAmountsRequest response = await ingredientService.GetTresholdAsync(request);
        ThresholdDto thresholds = new()
        {
            IndredientId = response.IngredientId,
            redAmount = response.RedAmount,
            yellowAmount = response.YellowAmount,
            redDaysUntil = response.RedDaysUntil,
            yellowDaysUntil = response.YellowDaysUntil
        };
        return thresholds;
    }

    public async Task<bool> UpdateTresholdsAsync(ThresholdDto thresholdDto)
    {
        UpdateWarningAmountsRequest request = new()
        {
            IngredientId = thresholdDto.IndredientId,
            RedAmount = thresholdDto.redAmount,
            YellowAmount = thresholdDto.yellowAmount,
            RedDaysUntil = thresholdDto.redDaysUntil,
            YellowDaysUntil = thresholdDto.yellowDaysUntil
        };
        Success response = await ingredientService.UpdateWarningAmountAsync(request);

        return response.Success_;
    }
    
    
}