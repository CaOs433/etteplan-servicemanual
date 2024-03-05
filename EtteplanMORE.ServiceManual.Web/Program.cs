using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using EtteplanMORE.ServiceManual.ApplicationCore.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IFactoryDeviceService, FactoryDeviceService>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();