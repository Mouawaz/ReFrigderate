using System.Text;
using System.Text.Json;
using APIContracts.UserDtos;

namespace BlazorApp1.Services;

public class HttpUserService: IUserService
{
    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }
    
    public async Task<UserDto> GetSingleUserAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"users/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<UserDto>();
        }
        else
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error retrieving user: {errorMessage}");
        }
    }

    public async Task<bool> AddUserAsync(CreateUserDto user)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync($"Users", user);
        if (response.IsSuccessStatusCode)
        {
            LoginResponseDto userInfo = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            return true;
        }
        else
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error retrieving user: {errorMessage}");
        }
    }

    public IQueryable<UserDto> GetAllUsers()
    {
        throw new NotImplementedException();
    }
}