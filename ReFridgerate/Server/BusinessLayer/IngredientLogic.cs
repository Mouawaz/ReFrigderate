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

    public async Task<IngredientDto> UpdateAsync(int id, UpdateIngredientDto ingredientInfo)
    {
        DateTime today = DateTime.Today.Date;
        if (ingredientInfo.Difference < 1)
        {
            throw new ArgumentException("Difference cannot be less than 1");
        }
        if (ingredientInfo.Substraction && ingredientInfo.amount < ingredientInfo.Difference)
        {
            throw new ArgumentException("The ingredient amount is less than the subtraction amount");
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
        return  clientManager.UpdateIngredient(ingredient, ingredientInfo.Difference * (ingredientInfo.Substraction == true ? -1 : 1));
    }

    public IQueryable<IngredientDto> GetAllIngredients()
    {
        return clientManager.GetAllIngredients();
    }

    public async Task<ThresholdDto> GetSingleTresholdAsync(int id)
    {
        if (id == null || id < 1)
        {
            throw new ArgumentException("Id cannot be null");
        }

        return await clientManager.GetTresholdAsync(id);
    }

    public async Task<bool> UpdateTresholdsAsync(int id, ThresholdDto thresholdDto)
    {
        if (id  == null || id < 1)
        {
            throw new ArgumentException("Id is invalid");
        }
        
        if (thresholdDto.redAmount < 0 || thresholdDto.yellowAmount < 0 || 
            thresholdDto.redDaysUntil < 0 || thresholdDto.yellowDaysUntil < 0)
        {
            throw new ArgumentException("Tresholds cannot be negative");
        }

        if (thresholdDto.redAmount >= thresholdDto.yellowAmount || thresholdDto.redDaysUntil >= thresholdDto.yellowDaysUntil)
        {
            throw new ArgumentException("Red tresholds cannot be greater or equal than yellow tresholds");
        }

        thresholdDto.IndredientId = id;
        return await clientManager.UpdateTresholdsAsync(thresholdDto);
    }
        public async Task<IngredientDto> CreateIngredient(CreateIngredientDto createIngredientDto) {
        if (createIngredientDto.Name.Length < 3) {
            throw new ArgumentException("The name is too short");
        }
        if (createIngredientDto.Cost < 0f) {
            throw new ArgumentException("The cost cannot be less then 0");
        }
        return await clientManager.CreateIngredient(createIngredientDto);
    }
}