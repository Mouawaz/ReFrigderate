using APIContracts.IngridientDtos;
using Entities;
using Google.Protobuf;
using Grpc.Net.Client;

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

            };
            ingredientDtos.Add(dto);
        }
        
        return ingredientDtos.AsQueryable();
    }
}