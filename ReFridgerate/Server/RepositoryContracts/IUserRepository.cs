using APIContracts.UserDtos;
using Entities;
using System.Linq;
namespace RepositoryContracts;

public interface IUserRepository
{
    Task<LoginResponseDto> AddAsync(CreateUserDto createUserDto);
    Task<UserDto> GetSingleAsync(int id);
    IQueryable<UserDto> GetMultiple();
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    Task<bool> UpdateAsync(int id, int role);
}