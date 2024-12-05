using System.Globalization;
using APIContracts.IngridientDtos;
using RepositoryContracts;
using Entities;
using GrpcClient;

namespace BusinessLayer;

public class IngredientLogic : IIngredientRepository
{
    public readonly IIngredientClientManager clientManager;

    public IngredientLogic(IIngredientClientManager clientManager)
    {
        this.clientManager = clientManager;
    }

    public async Task<IngredientDto> UpdateAsync(int id, UpdateIngredientDto ingredientInfo, int difference)
    {
        DateTime today = DateTime.Today.Date;
        if (ingredientInfo.amount.Equals(null) || ingredientInfo.Difference.Equals(null) ||
            ingredientInfo.Difference.Equals(null))
        {
            throw new ArgumentException("Data cannot be empty");
        }
        if (ingredientInfo.Substraction && ingredientInfo.amount < ingredientInfo.Difference)
        {
            throw new ArgumentException("The ingredient amount is less than the substraction amount");
        }
        DateTime givenDate;
        try
        {
             givenDate = DateTime.ParseExact(ingredientInfo.DateOfExpiration, "d/M/yyyy", CultureInfo.InvariantCulture);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new ArgumentException("Date is not valid");
        }
        
        IngredientDto ingredient = new()
        {
            Id = id,
            DaysUntilBad = (ingredientInfo.Substraction == true ? 0 : (givenDate - today).Days),
        };
        return  clientManager.UpdateIngredient(ingredient, difference * (ingredientInfo.Substraction == true ? -1 : 1));
    }

    public IQueryable<IngredientDto> GetAllIngredients()
    {
        return clientManager.GetAllIngredients();
    }
}