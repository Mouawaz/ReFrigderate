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
public class IngredientController : ControllerBase
{

    private readonly IIngredientRepository ingredientRepo;

    public IngredientController(IIngredientRepository ingredientRepo) {
        this.ingredientRepo = ingredientRepo;
    }
    [HttpPut("{id}")]
    public async Task<IResult> UpdateIngredient([FromRoute] int id,
        [FromBody] UpdateIngredientDto userInfo) {
        try {
            IngredientDto ingredient =
                await ingredientRepo.UpdateAsync(id, userInfo);
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
    
    [HttpPost]
    public async Task<IResult> CreateIngredient([FromBody] CreateIngredientDto dto) {
        try {
            IngredientDto ingredientDto = await ingredientRepo.CreateIngredient(dto);
            return Results.Ok(ingredientDto);
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
            IQueryable<IngredientDto> ingredients =
                ingredientRepo.GetAllIngredients();
            // List<IngredientDto> filteredIngredients = ingredients.OrderByDescending(i => i.StockStatus).ThenByDescending(i => i.ExpirationStatus).ToList();
            IEnumerable<IngredientDto> filteredIngredients =
                ingredients.Where(i => i.StockStatus == 3).ToList();
            filteredIngredients = filteredIngredients.Union(ingredients
                .Where(i => i.ExpirationStatus == 3).ToList());

            filteredIngredients = filteredIngredients.Union(ingredients
                .Where(i => i.StockStatus == 2).ToList());

            filteredIngredients = filteredIngredients.Union(ingredients
                .Where(i => i.ExpirationStatus == 2).ToList());

            filteredIngredients = filteredIngredients.Union(ingredients
                .Where(i => i.StockStatus == 1).ToList());

            filteredIngredients = filteredIngredients.Union(ingredients
                .Where(i => i.ExpirationStatus == 1).ToList());
            return Results.Ok(filteredIngredients);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return Results.BadRequest(e.Message);
        }
    }

    [HttpGet("/Ingredient/{id}/Thresholds")]
    public async Task<IResult> GetIngredientTresholds([FromRoute] int id) {
        try {
            ThresholdDto dto = await ingredientRepo.GetSingleTresholdAsync(id);
            return Results.Ok(dto);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return Results.BadRequest(e.Message);
        }
    }
    [HttpPut("/Ingredient/{id}/Thresholds")]
    public async Task<IResult> UpdateIngredientTresholds([FromRoute] int id,
        [FromBody] ThresholdDto ingredientThresholds) {
        try {
            bool success =
                await ingredientRepo.UpdateTresholdsAsync(id, ingredientThresholds);
            if (!success) {
                return Results.Problem("Failed to update ingredient tresholds");
            }
            return Results.Ok("Updated succesfully");
        }
        catch (ArgumentException e) {
            Console.WriteLine(e);

            return Results.BadRequest(e.Message);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return Results.StatusCode(500);
        }
    }
}