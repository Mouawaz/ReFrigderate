using GrpcClient2;

Client client = new Client();
Console.WriteLine(await client.GetSingleAsync(2)); 