using APIContracts.UserDtos;

namespace GrpcClient;

public interface IUserClientManager
{
    public Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    Task<LoginResponseDto> AddAsync(CreateUserDto userDto);
    Task<UserDto> GetSingleAsync(int id);
    IQueryable<UserDto> GetMultiple();
    Task<bool> UpdateUserAsync(int id, string role);

}