using System.Security.Claims;
using System.Text.Json;
using APIContracts.UserDtos;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlazorApp1.Components.Pages.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider 
{
    private readonly HttpClient _httpClient;
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());

    public SimpleAuthProvider(HttpClient httpClient, ProtectedLocalStorage protectedLocalStorage) 
    {
        _httpClient = httpClient;
        _protectedLocalStorage = protectedLocalStorage;
    }

    public async Task Login(string email, string password) 
    {
        var response = await _httpClient.PostAsJsonAsync("Auth", new LoginDto 
        {
            email = email,
            password = password
        });

        if (!response.IsSuccessStatusCode) 
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(
            await response.Content.ReadAsStringAsync(), 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        )!;

        await _protectedLocalStorage.SetAsync("userLogin", loginResponse);

        _currentUser = CreateClaimsPrincipal(loginResponse);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }

    public async Task Logout() 
    {
        await _protectedLocalStorage.DeleteAsync("userLogin");
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() 
    {
        try 
        {
            var storedLogin = await _protectedLocalStorage.GetAsync<LoginResponseDto>("userLogin");
            
            if (storedLogin.Success && storedLogin.Value != null)
            {
                _currentUser = CreateClaimsPrincipal(storedLogin.Value);
                return new AuthenticationState(_currentUser);
            }
        }
        catch 
        {
            // Log error if needed
        }

        return new AuthenticationState(_currentUser);
    }

    private ClaimsPrincipal CreateClaimsPrincipal(LoginResponseDto loginResponse)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, loginResponse.fullName),
            new Claim(ClaimTypes.Name, loginResponse.fullName)
        };
        
        var identity = new ClaimsIdentity(claims, "apiauth");
        return new ClaimsPrincipal(identity);
    }
}