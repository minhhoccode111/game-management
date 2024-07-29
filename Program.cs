// using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using GameManagementMvc.Models;
using GameManagementMvc.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GameManagementMvcContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("GameManagementMvcContext")
            ?? throw new InvalidOperationException(
                "Connection string 'GameManagementMvcContext' not found."
            )
    )
);

// dotnet ef migrations add InitialCreate
// dotnet ef database update
if (builder.Environment.IsDevelopment())
{
    // use sqlite in development and GameManagementMvcContext string in appsettings.json
    builder.Services.AddDbContext<GameManagementMvcContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("GameManagementMvcContext"))
    );
}
else
{
    // use ms sql in production and ProductionGameManagementMvcContext string in appsettings.json
    builder.Services.AddDbContext<GameManagementMvcContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ProductionGameManagementMvcContext"))
    );
}

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed database initilizer
// app.Servies: the application's configured services
// CreateScope: create a new IServiceScope that can be used to resolve scope services
using (var scope = app.Services.CreateScope())
{
    // The `IServiceProvider` used to resolve dependencies from the scope\.
    var services = scope.ServiceProvider;

    // call initialize method
    SeedDatabase.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// what is this?
app.UseHttpsRedirection();

app.UseStaticFiles();

// what is this?
app.UseRouting();

// what is this?
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
