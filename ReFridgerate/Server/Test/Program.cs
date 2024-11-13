using GrpcClient;

Client client = new Client();
IngredientClient ingredientClient = new IngredientClient();
ingredientClient.GetAllIngredients();
//Console.WriteLine(await client.GetSingleAsync(2)); 