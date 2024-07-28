using GameManagementMvc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GameManagementMvcContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("GameManagementMvcContext")
            ?? throw new InvalidOperationException(
                "Connection string 'GameManagementMvcContext' not found."
            )
    )
);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// TODO: add check development or production to use database and seed database

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
