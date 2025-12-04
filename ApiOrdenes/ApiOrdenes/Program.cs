using System;
using ApiOrdenes.BLL.Mapeos;
using ApiOrdenes.BLL.Servicios;
using ApiOrdenes.DLL;
using ApiOrdenes.DLL.Repositorio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Base de datos
builder.Services.AddDbContext<ApiContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Inyección de dependencias
builder.Services.AddScoped<IOrdenesRepositorio, OrdenesRepositorio>();
builder.Services.AddScoped<IOrdenesServicio, OrdenesServicio>();

// Cliente HTTP hacia API Inventario
builder.Services.AddHttpClient<IProductosServicio, ProductosServicio>(client =>
{
    // URL base del API de Inventario
    client.BaseAddress = new Uri(builder.Configuration["InventarioApi:BaseUrl"]);
});

builder.Services.AddAutoMapper(cfg => { }, typeof(MapeoClases));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

