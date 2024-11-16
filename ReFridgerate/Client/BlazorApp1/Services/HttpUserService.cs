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
}