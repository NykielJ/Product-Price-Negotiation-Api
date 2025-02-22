using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductPriceNegotiationApi.Data;
using ProductPriceNegotiationApi.Services;

var builder = WebApplication.CreateBuilder(args);

// DI container
builder.Services.AddSingleton<InMemoryProductRepository>();
builder.Services.AddSingleton<InMemoryNegotiationRepository>();
builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<NegotiationService>();

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

// If HTTP failed, remove it > dotnet clean > dotnet build > add it > dotnet clean then run 
app.UseRouting();
app.UseAuthorization();


app.MapControllers();


app.Run();
