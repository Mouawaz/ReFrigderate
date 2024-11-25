using System.Threading.Channels;
using APIContracts.UserDtos;
using Entities;
using Grpc.Net.Client;
using GrpcClient;
using RepositoryContracts;

namespace GrpcClient;

public class UserClient : IUserClientManager
{
    private GrpcChannel channel;
    private UserService.UserServiceClient userService;

    public UserClient()
    {
        channel = GrpcChannel.ForAddress("http://localhost:8080");
        userService = new UserService.UserServiceClient(channel);
    }

    /*public async Task<User> GetSingleAsync(int id)
    {
        UserIdRequest request = new UserIdRequest()
        {
            Userid = id.ToString()
        };
        User user = userService.GetUser(request);
        return user;
    }*/
    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        LoginRequest loginRequest = new()
        {
            Password = loginDto.password,
            Email = loginDto.email
        };
        
        LoginResponse loginResponse =  userService.AttemptLogin(loginRequest);

        LoginResponseDto loginResponseDto = new()
        {
            success = loginResponse.Success,
            userId = loginResponse.UserId,
            fullName = loginResponse.FullName,
        };
        return loginResponseDto;
    }
}