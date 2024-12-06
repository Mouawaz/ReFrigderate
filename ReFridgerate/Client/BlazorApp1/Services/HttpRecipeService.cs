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
    public async Task<List<Recipe>> GetRecipesAsync() {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync("Recipe");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        List<Recipe> recipes = new List<Recipe>();

        recipes = JsonSerializer.Deserialize<List<Recipe>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        Console.WriteLine(recipes);
        return recipes;
    }
}