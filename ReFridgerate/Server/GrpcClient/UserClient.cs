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
        
        LoginResponse loginResponse = await userService.AttemptLoginAsync(loginRequest);

        LoginResponseDto loginResponseDto = new()
        {
            success = loginResponse.Success,
            userId = loginResponse.UserId,
            fullName = loginResponse.FullName,
            role = loginResponse.Permissions
        };
        return loginResponseDto;
    }

    public async Task<LoginResponseDto> AddAsync(CreateUserDto userDto)
    {
        CreateUserRequest request = new()
        {
            Email = userDto.Email,
            Firstname = userDto.FirstName,
            Lastname = userDto.LastName,
            Password = userDto.Password
        };
        LoginResponse loginResponse = await userService.AddUserAsync(request);

        LoginResponseDto loginResponseDto = new()
        {
            success = loginResponse.Success,
            userId = loginResponse.UserId,
            fullName = loginResponse.FullName,
            role = loginResponse.Permissions
        };
        return loginResponseDto;
    }

    public async Task<UserDto> GetSingleAsync(int id)
    {
        UserRequest userRequest = new()
        {
            UserId = id
        };
       SingleUserResponse response = await userService.GetSingleUserAsync(userRequest);
       UserDto dto = new()
       {
           Id = response.Userid,
           Email = response.Email,
           FullName = response.FullName,
           Role = response.Role
       };
       return dto;
    }

    public  IQueryable<UserDto> GetMultiple()
    {
        EmptyUser empty = new();
        IQueryable<UserDto> users = userService.GetAllUsers(empty)
            .Messages.AsQueryable().Select(u => new UserDto
            {
                Id = u.Userid,
                Email = u.Email,
                FullName = u.FullName,
                Role = u.Role

            });
        return users;
    }

    public async Task<bool> UpdateUserAsync(int id, int role)
    {
        UpdateUserRequest request = new()
        {
            UserId = id,
            Role = role
        };
        UpdateUserResponse response = await userService.UpdateUserAsync(request);
        return response.Success;
    }
}