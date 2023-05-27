using Microsoft.EntityFrameworkCore;
using Tree;
using Tree.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<TreeDbContext>();
builder.Services.AddScoped<TreeSeeder>();

var app = builder.Build();

// Zainicjalizuj bazê danych i seeduj dane
SeedDatabase();


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<TreeDbContext>();
        var seeder = scope.ServiceProvider.GetRequiredService<TreeSeeder>();
        seeder.Seed(); // Wywo³anie metody Seed z klasy TreeSeeder
    }
}