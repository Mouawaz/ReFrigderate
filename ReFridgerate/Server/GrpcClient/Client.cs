using System.Threading.Channels;
using Entities;
using Grpc.Net.Client;
using GrpcClient;
using RepositoryContracts;

namespace GrpcClient;

public class Client : IUserRepository
{
    private GrpcChannel channel;
    private UserService.UserServiceClient userService;

    public Client()
    {
        channel = GrpcChannel.ForAddress("http://localhost:8080");
        userService = new UserService.UserServiceClient(channel);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        UserIdRequest request = new UserIdRequest()
        {
            Userid = id.ToString()
        };
        User user = userService.GetUser(request);
        return user;
    }
}