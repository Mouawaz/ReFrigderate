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

    public async Task<List<UserDto>> GetAllUsers()
    {
        HttpResponseMessage httpResponse = await client.GetAsync("Users");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        List<UserDto> users = new List<UserDto>();

        users = JsonSerializer.Deserialize<List<UserDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        Console.WriteLine(users);
        return users;
    }

    public async Task<bool> UpdateRecipeAsync(int id, int role)
    {
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"Users/{id}", role);

        if (!httpResponse.IsSuccessStatusCode)
        {
            string errorResponse = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }

        string response = await httpResponse.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<bool>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}