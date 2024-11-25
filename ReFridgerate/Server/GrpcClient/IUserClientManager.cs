using APIContracts.UserDtos;

namespace GrpcClient;

public interface IUserClientManager
{
    public Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
}