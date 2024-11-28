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
            UserDto dto = new UserDto()
            {
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                Sex = user.Sex
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
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                Sex = user.Sex
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
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            // new User  from the CreateUserDto
            //  the data from the DTO to our database entity
            var user = new User
            {
                Firstname = createUserDto.FirstName,
                Lastname = createUserDto.LastName,
                Email = createUserDto.Email,
                DateOfBirth = createUserDto.DateOfBirth,
                PhoneNumber = createUserDto.PhoneNumber,
                Sex = createUserDto.Sex
                // do we need some sort of password here?

            };



            var createdUser = await userRepo.AddAsync(user);


            // This is what we'll return to the client
            var userDto = new UserDto
            {
                FirstName = createdUser.Firstname,
                LastName = createdUser.Lastname,
                Email = createdUser.Email,
                DateOfBirth = createdUser.DateOfBirth,
                PhoneNumber = createdUser.PhoneNumber,
                Sex = createdUser.Sex
            };


            return CreatedAtAction(nameof(GetSingleUser), new { id = createdUser.Id }, userDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}