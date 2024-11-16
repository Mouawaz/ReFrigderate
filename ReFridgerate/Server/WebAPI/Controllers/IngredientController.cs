using APIContracts;
using APIContracts.IngridientDtos;
using Entities;
using Grpc.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class IngredientController{

    private readonly IIngredientRepository ingredientRepo;

    public IngredientController(IIngredientRepository ingredientRepo)
    {
        this.ingredientRepo = ingredientRepo;
    }
    [HttpPut("{id}")]
    public async Task<IResult> UpdateUser([FromRoute] int id,
        [FromBody] UpdateIngredientDto userInfo)
    {
        try
        {

            IngredientDto ingredient =
               await ingredientRepo.UpdateAsync(id, userInfo, userInfo.Difference);
            return Results.Ok(ingredient);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e);
            
            return Results.BadRequest(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.StatusCode(500);
        }
        
    }
    
    [HttpGet]
    public async Task<IResult> GetAllIngredients()
    {
        try
        {
            IQueryable<IngredientDto> ingredients = ingredientRepo.GetAllIngredients();
            return Results.Ok(ingredients);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(e.Message);
        }
    }
}