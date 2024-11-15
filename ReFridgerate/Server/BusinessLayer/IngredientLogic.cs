using System.Globalization;
using APIContracts.IngridientDtos;
using RepositoryContracts;
using Entities;
using GrpcClient;

namespace BusinessLayer;

public class IngredientLogic : IIngredientRepository
{
    public readonly IIngredientClientManager clientManager;

    public IngredientLogic()
    {
        this.clientManager = new IngredientClient();
    }

    public Ingredient UpdateAsync(int id, UpdateIngredientDto userInfo, int difference)
    {
        DateTime today = DateTime.Today.Date;
        DateTime givenDate;
        int daysUntilBad;
        if (userInfo.Amount < 0)
        {
             throw new ArgumentException("Amount cannot be negative");
        }
        try
        {
             givenDate = DateTime.ParseExact(userInfo.DateOfExpiration, "d/M/yyyy", CultureInfo.InvariantCulture);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new ArgumentException("Date is not valid");
        }
        
        Ingredient ingredient = new()
        {
            Id = id,
            Name = userInfo.Name,
            Amount = userInfo.Amount,
            Cost = userInfo.Cost,
            DaysUntilBad = (givenDate - today).Days
                
        };
        return  clientManager.UpdateIngredient(ingredient, difference);
    }

    public IQueryable<Ingredient> GetAllIngredients()
    {
        return clientManager.GetAllIngredients();
    }
}