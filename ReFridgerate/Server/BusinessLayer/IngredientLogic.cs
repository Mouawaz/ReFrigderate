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

    public async Task<IngredientDto> UpdateAsync(int id, UpdateIngredientDto userInfo, int difference)
    {
        DateTime today = DateTime.Today.Date;
        DateTime givenDate;
        try
        {
             givenDate = DateTime.ParseExact(userInfo.DateOfExpiration, "d/M/yyyy", CultureInfo.InvariantCulture);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new ArgumentException("Date is not valid");
        }
        
        IngredientDto ingredient = new()
        {
            Id = id,
            DaysUntilBad = (givenDate - today).Days,
        };
        return  clientManager.UpdateIngredient(ingredient, difference * (userInfo.Substraction == true ? -1 : 1));
    }

    public IQueryable<IngredientDto> GetAllIngredients()
    {
        return clientManager.GetAllIngredients();
    }
}