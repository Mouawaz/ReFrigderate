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
            User user = await userRepo.GetSingleAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            UserDto dto = new UserDto()
            {
                Id = user.Userid,
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Email = user.Email,
            };
            return dto;
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
            var users = userRepo.GetMultiple();
            var userDtos = users.Select(user => new UserDto
            {
                Id = user.Userid,
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Email = user.Email,
            });
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