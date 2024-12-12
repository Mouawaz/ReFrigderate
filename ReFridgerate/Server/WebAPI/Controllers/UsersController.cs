using APIContracts.UserDtos;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using System.Linq;

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
    public async Task<ActionResult<UserDto>> GetSingleUser([FromRoute] int id)
    {
        try
        {
            UserDto dto = await userRepo.GetSingleAsync(id);

            return Ok(dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IQueryable<UserDto>> GetAllUsers()
    {
        try
        {
            IQueryable<UserDto> userDtos = userRepo.GetMultiple();
            
            return Ok(userDtos);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponseDto>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            LoginResponseDto createdUser = await userRepo.AddAsync(createUserDto);

            return CreatedAtAction(nameof(GetSingleUser), new { id = createdUser.userId }, createUserDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateUser(
        [FromRoute] int id, [FromBody] int role)
    {
       bool success = await userRepo.UpdateAsync(id, role);
       return Ok(success);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var loginResponse = await userRepo.LoginAsync(loginDto);
            if (loginResponse.success)
            {
                return Ok(loginResponse);
            }
            else
            {
                return Unauthorized(loginResponse);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}