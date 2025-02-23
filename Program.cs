using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductPriceNegotiationApi.Data;
using ProductPriceNegotiationApi.Services;
using ProductPriceNegotiationApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=negotiation.db"));

// DI container
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<INegotiationRepository, NegotiationRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<NegotiationService>();

//Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Automatyczna migracja bazy danych
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// If HTTP failed, remove it > dotnet clean > dotnet build > add it > dotnet clean then run 
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
