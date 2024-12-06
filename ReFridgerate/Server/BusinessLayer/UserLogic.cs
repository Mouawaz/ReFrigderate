using APIContracts.UserDtos;
using Entities;
using GrpcClient;
using RepositoryContracts;

namespace BusinessLayer;

public class UserLogic : IUserRepository
{
    
    public readonly IUserClientManager clientManager;

    public UserLogic(IUserClientManager clientManager)
    {
        this.clientManager = clientManager;
    }
    public async Task<UserDto> GetSingleAsync(int id)
    {
        UserDto user = await clientManager.GetSingleAsync(id);

        if (id.Equals(null) || id < 1)
        {
            throw new ArgumentException("Provided user id is not valid");
        }

        return user;
    }

    public IQueryable<UserDto> GetMultiple()
    {
       return clientManager.GetMultiple();
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        if (loginDto.email.Equals(null) || loginDto.password.Equals(null))
        {
            throw new ArgumentException("Fields cannot be empty");
            
        }
        else if (!loginDto.email.Contains("@") && loginDto.password.Contains("."))
        {
            throw new  ArgumentException("Invalid email");
        }
        else if (loginDto.password.Length < 5 )
        {
            throw new  ArgumentException("Password has to be longer than 5 characters");
        }
        else if (loginDto.password.All(Char.IsDigit))
        {
            throw new ArgumentException("Password cannot contain only digits");
        }
        
        return await clientManager.LoginAsync(loginDto);
        
    }

    public async Task<bool> UpdateAsync(int id, string role)
    {
        if (id.Equals(null) || id < 1 || role.Equals(null))
        {
            throw new ArgumentException("Provided information is not valid");
        }
        return await clientManager.UpdateUserAsync(id, role);
    }
    

    public async Task<LoginResponseDto> AddAsync(CreateUserDto userDto)
    {
        if (userDto.FirstName == null || userDto.LastName == null || userDto.Email == null || userDto.Password == null)
        {
            throw new ArgumentException("Fields cannot be empty");
            
        }

        if (userDto.FirstName.Any(char.IsDigit ) || userDto.LastName.Any(char.IsDigit))
        {
            throw new ArgumentException("First and Last name cannot contain digits");
        }
        return await clientManager.AddAsync(userDto);
    }
}