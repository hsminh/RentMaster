using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using RentMaster.Data;
using RentMaster.Accounts;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env
Env.Load();

// Build PostgreSQL connection string
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register Controllers
builder.Services.AddControllers();

// Register custom modules
builder.Services.AddAccountModule();

// Register OpenAPI/Swagger
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // enable Swagger UI for dev
}

app.UseHttpsRedirection();
app.MapControllers();

// Example route
app.MapGet("/weatherforecast", () =>
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm",
            "Balmy", "Hot", "Sweltering", "Scorching"
        };

        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast(
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            )).ToArray();

        return forecast;
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}