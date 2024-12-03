using APIContracts.IngridientDtos;
using APIContracts.UserDtos;
using BusinessLayer;
using Entities;
using GrpcClient;
using RepositoryContracts;
using WebAPI;
using WebAPI.Controllers;

IIngredientRepository repository = new IngredientLogic(new IngredientClient()); 
IngredientController controller = new IngredientController(repository);
IUserRepository userRepository = new UserLogic(new UserClient());
AuthController authController = new AuthController(userRepository);
UserClient userClient = new UserClient();
IngredientClient ingredientClient = new IngredientClient();
//ingredientClient.GetAllIngredients();
/*UpdateIngredientDto dto = new()
{
    Name = "Carrots",
    Amount = 100,
    Difference = 10,
    Cost = 120,
    Substraction = false,
    DateOfExpiration = DateTime.Now.Date.ToString("d/M/yyyy")
    
    
};
var result = controller.UpdateIngredient(1, dto).Result;*/

LoginDto loginDto = new("jdoe@example.com","password123")
{

};
var result = await authController.CheckUser(loginDto);
Console.WriteLine(result);

//Console.WriteLine(await client.GetSingleAsync(2)); 