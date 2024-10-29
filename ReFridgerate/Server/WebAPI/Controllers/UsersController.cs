
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }
    

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetSingleUser([FromRoute] int id)
    {
        try
        {
            User user = await userRepo.GetSingleAsync(id);
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
             return StatusCode(500, e.Message);
        }
    }
}