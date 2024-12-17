using APIContracts.IngridientDtos;
using APIContracts.RecipeDtos;
using BusinessLayer;
using FakeItEasy;
using FluentAssertions;
using GrpcClient;
using RepositoryContracts;

namespace ServerTests;

public class RecipeLogicTests
{
    private RecipeLogic recipeLogic;
    private IRecipeClientManager clientManager;

    public RecipeLogicTests()
    {
        clientManager = A.Fake<IRecipeClientManager>();
        recipeLogic = new RecipeLogic(clientManager);
    }

    [Fact]
    public void RecipeLogic_AddAsync_ReturnsRecipeDto()
    {
        CreateRecipeDto recipeDto = new CreateRecipeDto()
        {
            name = "Pasta bologna",
            instructions = "Cook pasta in boiling water for 5 min and later mix with chopped tomatoes",
            type = "Main",
            creatorId = 1,
            modificationsAllowed = false,
            ingredients = new List<RecipeIngredientDto>()
            {
                new RecipeIngredientDto()
                {
                    IngredientId = 1,
                    Cost = 3,
                    IngredientName = "Tomato",
                    Quantity = 12,
                },
                new RecipeIngredientDto()
                {
                    IngredientId = 2,
                    Cost = 2,
                    Quantity = 3,
                    IngredientName = "Pasta"
                }
            }
        };
        A.CallTo(() => clientManager.AddAsync(recipeDto)).Returns(A.Fake<RecipeDto>());
        
        var result = recipeLogic.AddAsync(recipeDto);

        result.Should().BeOfType<Task<RecipeDto>>();
    }
    [Fact]
    public async Task RecipeLogic_AddAsync_ReturnsDataCannotBeNull()
    {
        CreateRecipeDto recipeDto = new CreateRecipeDto()
        {
            name = "Pasta bologna",
            instructions = "Cook pasta in boiling water for 5 min and later mix with chopped tomatoes",
            creatorId = 1,
            modificationsAllowed = false,
            ingredients = new List<RecipeIngredientDto>()
            {
                new RecipeIngredientDto()
                {
                    IngredientId = 1,
                    Cost = 3,
                    IngredientName = "Tomato",
                    Quantity = 12,
                },
                new RecipeIngredientDto()
                {
                    IngredientId = 2,
                    Cost = 2,
                    Quantity = 3,
                    IngredientName = "Pasta"
                }
            }
        };
        
        Func<Task> act = async () => await recipeLogic.AddAsync(recipeDto);

       await act.Should().ThrowAsync<ArgumentException>().WithMessage("Data cannot be null");
    }
    
    [Fact]
    public async Task RecipeLogic_AddAsync_RecipeIngredientEmpty()
    {
        CreateRecipeDto recipeDto = new CreateRecipeDto()
        {
            name = "Pasta bologna",
            instructions = "Cook pasta in boiling water for 5 min and later mix with chopped tomatoes",
            creatorId = 1,
            type = "Main",
            modificationsAllowed = false,
            ingredients = new List<RecipeIngredientDto>()
        };
        
        Func<Task> act = async () => await recipeLogic.AddAsync(recipeDto);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Recipe ingredients cannot be empty");
    }
    [Fact]
    public async Task RecipeLogic_AddAsync_InvalidIngredientInfo()
    {
        CreateRecipeDto recipeDto = new CreateRecipeDto()
        {
            name = "Pasta bologna",
            instructions = "Cook pasta in boiling water for 5 min and later mix with chopped tomatoes",
            creatorId = 1,
            type = "Main",
            modificationsAllowed = false,
            ingredients = new List<RecipeIngredientDto>()
            {
                new RecipeIngredientDto()
                {
                    IngredientId = 1,
                    Cost = 3,
                    IngredientName = "Tomato",
                    Quantity = 12,
                },
                new RecipeIngredientDto()
                {
                }
            }
        };
        
        Func<Task> act = async () => await recipeLogic.AddAsync(recipeDto);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid recipe ingredient information");
    }
    
    [Fact]
    public void RecipeLogic_UpdateAsync_ReturnsRecipeDto()
    {
        int id = 1;
        CreateRecipeDto recipeDto = new CreateRecipeDto()
        {
            name = "Pasta bologna",
            instructions = "Cook pasta in boiling water for 5 min and later mix with chopped tomatoes",
            type = "Main",
            creatorId = 1,
            modificationsAllowed = false,
            ingredients = new List<RecipeIngredientDto>()
            {
                new RecipeIngredientDto()
                {
                    IngredientId = 1,
                    Cost = 3,
                    IngredientName = "Tomato",
                    Quantity = 12,
                },
                new RecipeIngredientDto()
                {
                    IngredientId = 2,
                    Cost = 2,
                    Quantity = 3,
                    IngredientName = "Pasta"
                }
            }
        };
        A.CallTo(() => clientManager.UpdateRecipeAsync(id, recipeDto)).Returns(A.Fake<RecipeDto>());
        
        var result = recipeLogic.UpdateAsync(id, recipeDto);

        result.Should().BeOfType<Task<RecipeDto>>();
    }
    [Theory]
    [InlineData(null, "Main")]
    [InlineData("Pasta bologna",null)]
    public async Task RecipeLogic_UpdateAsync_ReturnsDataCannotBeNull(String name,  String type)
    {
        CreateRecipeDto recipeDto = new CreateRecipeDto()
        {
            name = name,
            creatorId = 1,
            modificationsAllowed = false,
            type = type,
            ingredients = new List<RecipeIngredientDto>()
            {
                new RecipeIngredientDto()
                {
                    IngredientId = 1,
                    Cost = 3,
                    IngredientName = "Tomato",
                    Quantity = 12,
                },
                new RecipeIngredientDto()
                {
                    IngredientId = 2,
                    Cost = 2,
                    Quantity = 3,
                    IngredientName = "Pasta"
                }
            }
        };
        int recipeId = 1;
        
        Func<Task> act = async () => await recipeLogic.UpdateAsync(recipeId, recipeDto);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Data cannot be null");
    }
    
    [Theory]
    [InlineData(null, 1,"Main")]
    [InlineData(1,null,"Main")]
    [InlineData(1,1,"Main123")]
    public async Task RecipeLogic_UpdateAsync_InvalidRecipeInfo(int id, int creatorId, string type)
    {
        CreateRecipeDto recipeDto = new CreateRecipeDto()
        {
            name = "Pasta bologna",
            creatorId = creatorId,
            modificationsAllowed = false,
            type = type,
            ingredients = new List<RecipeIngredientDto>()
            {
                new RecipeIngredientDto()
                {
                    IngredientId = 1,
                    Cost = 3,
                    IngredientName = "Tomato",
                    Quantity = 12,
                },
                new RecipeIngredientDto()
                {
                    IngredientId = 2,
                    Cost = 2,
                    Quantity = 3,
                    IngredientName = "Pasta"
                }
            }
        };
        
        Func<Task> act = async () => await recipeLogic.UpdateAsync(id, recipeDto);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid recipe information");
    }
    [Theory]
    [InlineData(null,"Main")]
    [InlineData(1,null)]
    public async Task RecipeLogic_UpdateAsync_InvalidIngredientInfo(int id, string name)
    {
        CreateRecipeDto recipeDto = new CreateRecipeDto()
        {
            name = "Pasta bologna",
            creatorId = 1,
            modificationsAllowed = false,
            type = "Main",
            ingredients = new List<RecipeIngredientDto>()
            {
                new RecipeIngredientDto()
                {
                    IngredientId = id,
                    Cost = 3,
                    IngredientName = name,
                    Quantity = 12,
                },
                new RecipeIngredientDto()
                {
                    IngredientId = 2,
                    Cost = 2,
                    Quantity = 3,
                    IngredientName = "Pasta"
                }
            }
        };
        int recipeId = 1;
        
        Func<Task> act = async () => await recipeLogic.UpdateAsync(1, recipeDto);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid recipe ingredient information");
    }

    [Fact]
    public async Task RecipeLogic_DeleteAsync_ReturnsException()
    {
        int id = 0;
        
        Func<Task> act = async () => await recipeLogic.DeleteAsync(id);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Id cannot be null or negative");
    }
}