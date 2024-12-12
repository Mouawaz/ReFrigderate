using System.Text.Json;
using APIContracts.IngridientDtos;
using BlazorApp1.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BlazorApp.Components.Services
{
    public class HttpIngredientService: IIngredientService
    {
        private readonly HttpClient client;

        public HttpIngredientService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<List<IngredientDto>> GetIngredientsAsync()
        {
            HttpResponseMessage httpResponse = await client.GetAsync("ingredient");
            string response = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception(response);
            }

            return JsonSerializer.Deserialize<List<IngredientDto>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }

        public async Task<IngredientDto> AddIngredientAsync(IngredientDto ingredient)
        {
            throw new NotImplementedException();
        }
        
        public async Task<IngredientDto> UpdateIngredientAsync(int id, UpdateIngredientDto updateIngredient)
        {
            HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"ingredient/{id}", updateIngredient);

            if (!httpResponse.IsSuccessStatusCode)
            {
                string errorResponse = await httpResponse.Content.ReadAsStringAsync();
                throw new Exception(errorResponse);
            }

            string response = await httpResponse.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<IngredientDto>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }
        public async Task<ThresholdDto> GetThresholds(int id) {
            HttpResponseMessage httpResponse = await client.GetAsync($"ingredient/Treshold/{id}");
            string response = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception(response);
            }

            return JsonSerializer.Deserialize<ThresholdDto>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }
        public async Task UpdateThreshold(int id, ThresholdDto updateThreshold) {
            HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"IngredientTresholds/{id}", updateThreshold);

            if (!httpResponse.IsSuccessStatusCode)
            {
                string errorResponse = await httpResponse.Content.ReadAsStringAsync();
                throw new Exception(errorResponse);
            }

            // string response = await httpResponse.Content.ReadAsStringAsync();
        }
    }
}