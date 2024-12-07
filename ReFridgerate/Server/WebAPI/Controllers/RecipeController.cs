using APIContracts;
using APIContracts.IngridientDtos;
using APIContracts.RecipeDtos;
using Entities;
using Grpc.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;



[ApiController]
[Route("[controller]")]
public class RecipeController : ControllerBase
{
    private readonly IRecipeRepository recipeRepo;

    public RecipeController(IRecipeRepository recipeRepo)
    {
        this.recipeRepo = recipeRepo;
    }
    
    [HttpGet]
    public async Task<IResult> GetAllRecipes()
    {
        try
        {
            IQueryable<RecipeDto> recipes =
                recipeRepo.GetAllRecipes();
            return Results.Ok(recipes);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(e.Message);
        }
    }
    [HttpPost]
    public async Task<ActionResult<RecipeDto>> CreateRecipe([FromBody]  CreateRecipeDto createRecipeDto )
    {
        try
        {
            

            RecipeDto recipe = await recipeRepo.AddAsync(createRecipeDto);

            return Created("Recipe", recipe);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RecipeDto>> UpdateUser([FromRoute] int id,
        [FromBody] CreateRecipeDto recipeDto)
    {
        RecipeDto updatedRecipe = await recipeRepo.UpdateAsync(id, recipeDto);
       return Ok(updatedRecipe);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRecipe([FromRoute]int id)
    {
        try
        {
            await recipeRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}