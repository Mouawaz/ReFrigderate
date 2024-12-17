using BlazorApp.Components.Services;
using BlazorApp1.Components;
using BlazorApp1.Components.Pages.Auth;
using BlazorApp1.Hubs;
using BlazorApp1.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5180")
});

// Add Authentication Services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login"; 
        options.LogoutPath = "/logout"; 
        options.ExpireTimeSpan = TimeSpan.FromDays(7); 
        options.SlidingExpiration = true; 
    });

// Add Authorization
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IThemeState, ThemeState>();
builder.Services.AddScoped<IUserService, HttpUserService>();
builder.Services.AddScoped<IIngredientService, HttpIngredientService>();
builder.Services.AddScoped<IRecipeService, HttpRecipeService>();
builder.Services.AddScoped<AuthenticationStateProvider, SimpleAuthProvider>();

builder.Services.AddScoped<OrdersService>();
//Necessary for signalr
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});


var app = builder.Build();
app.UseResponseCompression();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Add Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapHub<OrdersHub>("/ordershub");
app.Run();