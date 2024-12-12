using APIContracts.UserDtos;

namespace BlazorApp1.Services;


public interface IUserService
{
    Task<UserDto> GetSingleUserAsync(int id);
    Task<bool> AddUserAsync(CreateUserDto user);
    IQueryable<UserDto> GetAllUsers();
}