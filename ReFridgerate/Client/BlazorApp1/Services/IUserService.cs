using APIContracts.UserDtos;

namespace BlazorApp1.Services;


public interface IUserService
{
    Task<UserDto> GetSingleUserAsync(int id);
    Task<bool> AddUserAsync(CreateUserDto user);
    Task<List<UserDto>> GetAllUsers();
    Task<bool> UpdateRecipeAsync(int id, int role);

}