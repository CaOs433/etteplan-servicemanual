using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using EtteplanMORE.ServiceManual.ApplicationCore.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database

string dbHost = Environment.GetEnvironmentVariable("DB_HOST")!;
string dbPort = Environment.GetEnvironmentVariable("DB_PORT")!;
string dbDatabase = Environment.GetEnvironmentVariable("DB_DATABASE")!;
string dbUser = Environment.GetEnvironmentVariable("DB_USER")!;
string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD")!;

string connectionString = $"Host={dbHost};Port={dbPort};Database={dbDatabase};username={dbUser};Password={dbPassword};";

builder.Services.AddDbContext<FactoryDeviceDbContext>(optionsBuilder =>
    optionsBuilder.UseNpgsql(connectionString)
);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IFactoryDeviceService, FactoryDeviceService>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<FactoryDeviceDbContext>();

    // Apply pending migrations to the database
    dbContext.Database.Migrate();

    // Seed the database with initial data if not already seeded
    SeedData.Initialize(services);
}

app.Run();