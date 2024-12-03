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
            IQueryable<Recipe> recipes =
                recipeRepo.GetAllRecipes();
            return Results.Ok(recipes);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(e.Message);
        }
    }
}