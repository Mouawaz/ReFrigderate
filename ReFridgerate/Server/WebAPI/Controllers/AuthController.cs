using APIContracts.UserDtos;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public AuthController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }
    
    [HttpPost]
    public async Task<ActionResult<LoginResponseDto>> CheckUser(
        [FromBody] LoginDto loginDto)
    {
        try
        {
            return await userRepo.LoginAsync(loginDto);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(400, e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}