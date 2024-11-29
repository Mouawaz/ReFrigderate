using System.Security.Claims;
using System.Text.Json;
using APIContracts.UserDtos;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlazorApp1.Components.Pages.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider {
    private readonly HttpClient httpClient;
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private ClaimsPrincipal currentClaimsPrincipal;

    public SimpleAuthProvider(HttpClient httpClient, ProtectedLocalStorage protectedLocalStorage) {
        this.httpClient = httpClient;
        _protectedLocalStorage = protectedLocalStorage;
    }

    public async Task Login(string email, string password) {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            "Auth",
            new LoginDto(email, password));
        string content = await response.Content.ReadAsStringAsync();
        Console.WriteLine("a");
        if (!response.IsSuccessStatusCode) {
            Console.WriteLine("a1");
            throw new Exception(content);
        }
        Console.WriteLine("b");
        LoginResponseDto loginResponseDto = JsonSerializer.Deserialize<LoginResponseDto>(content, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        })!;

        // Store login information in protected local storage
        // await _protectedLocalStorage.SetAsync("userLogin", userDto);
        Console.WriteLine("c");
        List<Claim> claims = new List<Claim>() {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, loginResponseDto.fullName) // Using email as name
        };
        Console.WriteLine("d");
        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
        Console.WriteLine("e");
        currentClaimsPrincipal = new ClaimsPrincipal(identity);
        Console.WriteLine("f");
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(currentClaimsPrincipal))
        );
    }

    public async Task Logout() {
        // Remove stored login information
        await _protectedLocalStorage.DeleteAsync("userLogin");

        currentClaimsPrincipal = new(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentClaimsPrincipal)));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
        // try {
        //     // Try to retrieve stored login information
        //     var storedLogin = await _protectedLocalStorage.GetAsync<LoginDto>("userLogin");
        //     return new AuthenticationState(currentClaimsPrincipal);
        //     if (storedLogin.Success && storedLogin.Value != null)
        //     {
        //         var claims = new List<Claim>
        //         {
        //             new Claim(ClaimTypes.Email, storedLogin.Value.email),
        //             new Claim(ClaimTypes.Name, storedLogin.Value.email)
        //         };
        //     
        //         var identity = new ClaimsIdentity(claims, "apiauth");
        //         currentClaimsPrincipal = new ClaimsPrincipal(identity);
        //     
        //         return new AuthenticationState(currentClaimsPrincipal);
        //     }
        // }
        // catch {
        //     // In case of any error, return unauthenticated state
        //     Console.WriteLine("diuwahiuadwiu");
        //     return new AuthenticationState(new ClaimsPrincipal());
        // }
        return new AuthenticationState(currentClaimsPrincipal ?? new());
    }
}