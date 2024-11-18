using System.Text.Json;
using APIContracts.IngridientDtos;
using BlazorApp1.Services;

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
    }
}