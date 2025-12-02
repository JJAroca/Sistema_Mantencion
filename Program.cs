using Microsoft.EntityFrameworkCore;
using SistemaMantencion.Data;
using SistemaMantencion.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar DbContext con SQLite
builder.Services.AddDbContext<MantencionDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar gRPC Server
builder.Services.AddGrpc();

// Configurar gRPC Client para comunicarse con Sistema Comercial
builder.Services.AddGrpcClient<ArriendoGrpc.ArriendoService.ArriendoServiceClient>(options =>
{
    options.Address = new Uri("https://localhost:7001"); // Puerto del Sistema Comercial
});

var app = builder.Build();

// Crear base de datos si no existe
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MantencionDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Mapear endpoints gRPC
app.MapGrpcService<MantencionServiceImpl>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();