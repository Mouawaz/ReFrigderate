using System.Security.Claims;
using System.Text.Json;
using APIContracts.UserDtos;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlazorApp1.Components.Pages.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private ClaimsPrincipal currentClaimsPrincipal = new(new ClaimsIdentity());

    public SimpleAuthProvider(HttpClient httpClient, ProtectedLocalStorage protectedLocalStorage)
    {
        this.httpClient = httpClient;
        _protectedLocalStorage = protectedLocalStorage;
    }

    public async Task Login(string email, string password)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            "auth/login",
            new LoginDto(email, password));

        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        LoginDto userDto = JsonSerializer.Deserialize<LoginDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        // Store login information in protected local storage
        await _protectedLocalStorage.SetAsync("userLogin", userDto);

        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, userDto.email),
            new Claim(ClaimTypes.Name, userDto.email) // Using email as name
        };
        
        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
        currentClaimsPrincipal = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(currentClaimsPrincipal))
        );
    }
    
    public async Task Logout()
    {
        // Remove stored login information
        await _protectedLocalStorage.DeleteAsync("userLogin");

        currentClaimsPrincipal = new(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentClaimsPrincipal)));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Try to retrieve stored login information
            var storedLogin = await _protectedLocalStorage.GetAsync<LoginDto>("userLogin");
            
            if (storedLogin.Success && storedLogin.Value != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, storedLogin.Value.email),
                    new Claim(ClaimTypes.Name, storedLogin.Value.email)
                };

                var identity = new ClaimsIdentity(claims, "apiauth");
                currentClaimsPrincipal = new ClaimsPrincipal(identity);

                return new AuthenticationState(currentClaimsPrincipal);
            }
        }
        catch
        {
            // In case of any error, return unauthenticated state
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }
}