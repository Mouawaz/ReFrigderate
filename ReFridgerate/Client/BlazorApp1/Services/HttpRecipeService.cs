using System.Text.Json;
using APIContracts.RecipeDtos;

namespace BlazorApp1.Services;

public class HttpRecipeService : IRecipeService {
    private readonly HttpClient _httpClient;
    public HttpRecipeService(HttpClient httpClient) {
        _httpClient = httpClient;
    }
    public async Task<Recipe> AddRecipeAsync(CreateRecipeDto recipeDto) {
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("Recipe", recipeDto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<Recipe>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
    public Task<List<Recipe>> GetRecipesAsync() {
        throw new NotImplementedException();
    }
}