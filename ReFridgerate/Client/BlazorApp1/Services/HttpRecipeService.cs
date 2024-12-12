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
    public async Task<List<RecipeDto>> GetRecipesAsync() {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync("Recipe");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        List<RecipeDto> recipes = new List<RecipeDto>();

        recipes = JsonSerializer.Deserialize<List<RecipeDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        Console.WriteLine(recipes);
        return recipes;
    }
    
    public async Task DeleteRecipeAsync(int id)
    {
        HttpResponseMessage httpResponse = await _httpClient.DeleteAsync($"Recipe/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }
    
    public async Task<RecipeDto> UpdateRecipeAsync(int id, RecipeDto updateRecipe)
    {
        Console.WriteLine($"this is my shit {JsonSerializer.Serialize(updateRecipe)}");
        HttpResponseMessage httpResponse = await _httpClient.PutAsJsonAsync($"Recipe/{id}", updateRecipe);

        if (!httpResponse.IsSuccessStatusCode)
        {
            string errorResponse = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }

        string response = await httpResponse.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<RecipeDto>(response, new JsonSerializerOptions
            {
            PropertyNameCaseInsensitive = true
        })!;
    }

    
    
}