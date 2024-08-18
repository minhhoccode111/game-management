using GameManagementMvc.Data;
using GameManagementMvc.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// add database context and cache
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<GameManagementMvcContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'MyDb' not found.")
        )
    );
    builder.Services.AddDistributedMemoryCache();
}
else
{
    builder.Services.AddDbContext<GameManagementMvcContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")
        )
    );
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration["AZURE_REDIS_CONNECTIONSTRING"];
        options.InstanceName = "game-management-cache";
    });
}

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedDatabase.Initialize(services);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
