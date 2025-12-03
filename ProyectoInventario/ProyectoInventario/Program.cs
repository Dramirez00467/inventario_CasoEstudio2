using ProyectoInventarioBLL.Mapeos;
using ProyectoInventarioBLL.Servicios;
using ProyectoInventarioDAL.Repositorios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddSingleton<IProductosRepositorio, ProductosRepositorio>();
builder.Services.AddSingleton<IProductosServicio, ProductosServicio>();
builder.Services.AddSingleton<IOrdenesRepositorio, OrdenesRepositorio>();
builder.Services.AddSingleton<IOrdenesServicio, OrdenesServicio>();

builder.Services.AddAutoMapper(cfg => { }, typeof(MapeoClases));

builder.Services.AddHttpClient<IProductosRepositorio, ProductosRepositorio>();
builder.Services.AddHttpClient<IOrdenesRepositorio, OrdenesRepositorio>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Productos",
    pattern: "{controller=Productos}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Ordenes",
    pattern: "{controller=Ordenes}/{action=Index}/{id?}");

app.Run();
