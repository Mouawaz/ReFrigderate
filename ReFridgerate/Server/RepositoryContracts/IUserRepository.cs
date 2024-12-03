using APIContracts.UserDtos;
using Entities;
using System.Linq;
namespace RepositoryContracts;

public interface IUserRepository
{
    Task<LoginResponseDto> AddAsync(CreateUserDto createUserDto);
    Task<User> GetSingleAsync(int id);
    IQueryable<User> GetMultiple();
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
}