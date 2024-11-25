using APIContracts.UserDtos;
using Entities;
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
     //For now these are just placeholders, until changes are made
    /*
    [HttpGet]
    public async Task<ActionResult<IQueryable<UserDto>>> GetAllUsers()
    {
        // Todo be implemented with getmany() 
        
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        // Todo AddAsync() 
        
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUser([FromRoute] int id, [FromBody] UpdateUserDto updateUserDto)
    {
        // Todo with UpdateAsync() 
        
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser([FromRoute] int id)
    {
        // Todo  with DeleteAsync() 
        
    }
    */
}