using GrpcClient;

Client client = new Client();
Console.WriteLine(await client.GetSingleAsync(2)); 