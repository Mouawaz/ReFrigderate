using APIContracts.UserDtos;
using Entities;
using GrpcClient;
using RepositoryContracts;

namespace BusinessLayer;

public class UserLogic : IUserRepository
{
    
    public readonly IUserClientManager clientManager;

    public UserLogic()
    {
        this.clientManager = new UserClient();
    }
    /*public Task<User> GetSingleAsync(int id)
    {
        throw new NotImplementedException();
    }*/
    
    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        if (!loginDto.email.Contains("@") && loginDto.password.Contains("."))
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

    public Task<User> GetSingleAsync(int id)
    {
        throw new NotImplementedException();
    }
}