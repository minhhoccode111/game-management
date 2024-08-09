// using Microsoft.Extensions.DependencyInjection;
using GameManagementMvc.Data;
using GameManagementMvc.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// inject database context
builder.Services.AddDbContext<GameManagementMvcContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'MyDb' not found.")
    )
);

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

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
