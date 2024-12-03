using BusinessLayer;
using GrpcClient;
using RepositoryContracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepository, UserLogic>();
builder.Services.AddScoped<IIngredientRepository, IngredientLogic>();
builder.Services.AddScoped<IRecipeRepository, RecipeLogic>();


builder.Services.AddScoped<IIngredientClientManager, IngredientClient>();
builder.Services.AddScoped<IUserClientManager, UserClient>();
builder.Services.AddScoped<IRecipeClientManager, RecipeClient>();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();