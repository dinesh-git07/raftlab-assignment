using RaftLab_Assignment.Interfaces;
using RaftLab_Assignment.Services;
using Polly;
using Polly.Extensions.Http;
using RaftLab_Assignment.Configuration;
using Microsoft.Extensions.Options;

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError() 
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound) 
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), 
            onRetry: (outcome, timespan, retryCount, context) =>
            {
                
                Console.WriteLine($"Retrying {retryCount} after {timespan.TotalSeconds} seconds due to {outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()}");
            }
        );
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();


builder.Services.Configure<ReqResAPISettings>(builder.Configuration.GetSection("ReqResApi"));

builder.Services.AddControllers(); 
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<IReqResInterface, ReqResClient>((serviceProvider, client) =>
{
    // Get settings from configuration
    var reqResSettings = serviceProvider.GetRequiredService<IOptions<ReqResAPISettings>>().Value;
    client.BaseAddress = new Uri(reqResSettings.BaseUrl);

    // Add default headers if needed
    if (!string.IsNullOrEmpty(reqResSettings.ApiKey))
    {
        client.DefaultRequestHeaders.Add("x-api-key", $"{reqResSettings.ApiKey}");
    }
})
.AddPolicyHandler(GetRetryPolicy());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
