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

    public Ingredient UpdateIngredient(Ingredient ingredient, int difference)
    {
        UpdateIngredientRequest request = new()
        {
            Id = ingredient.Id,
            Amount = ingredient.Amount,
            DaysUntilBad = ingredient.DaysUntilBad,
            Difference = difference
        };
        return ingredientService.UpdateIngredient(request);
    }

    public IQueryable<Ingredient> GetAllIngredients()
    {
        Console.WriteLine( ingredientService.GetAllIngredients(new Empty()));
        return ingredientService.GetAllIngredients(new Empty()).Messages.AsQueryable();
    }
}