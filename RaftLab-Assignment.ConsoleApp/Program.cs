using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using RaftLab_Assignment;
using RaftLab_Assignment.Interfaces;
using RaftLab_Assignment.Models;
using RaftLab_Assignment.Services;
using RaftLab_Assignment.Utils;

var services = new ServiceCollection();

// Add HttpClient for ReqResClient
services.AddHttpClient<IReqResInterface, ReqResClient>(client =>
{
    client.BaseAddress = new Uri("https://reqres.in/");
    client.DefaultRequestHeaders.Add("x-api-key", "reqres-free-v1");
});

var provider = services.BuildServiceProvider();
var client = provider.GetRequiredService<IReqResInterface>();

try
{
    Console.WriteLine("Get All Users...");
    var users = await client.GetAllUsersAsync(1);
    foreach (var user in users)
    {
        Console.WriteLine($"{user.id}: {user.first_name} {user.last_name} - {user.email}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

try
{
    Console.WriteLine("\n\nGet User by id 1...");
    var user = await client.GetUserByIdAsync(1);
    
    Console.WriteLine($"{user.id}: {user.first_name} {user.last_name} - {user.email}");
    
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
