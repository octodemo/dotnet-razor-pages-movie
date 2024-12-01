#define DEFAULT // SQL server is default, SQL_Lite is other
#if DEFAULT
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure Data Protection to use a file-based key store
builder.Services.AddDataProtection()
    .SetApplicationName("RazorPagesMovie")
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "keys")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.Use(async (context, next) =>
{
    var path = context.Request.Path;
    var isAccountPage = path.StartsWithSegments("/Account");
    var isAuthenticated = context.Session.GetInt32("UserId").HasValue;

    if (!isAuthenticated && !isAccountPage && !path.StartsWithSegments("/Index"))
    {
        context.Response.Redirect("/Account/Login");
        return;
    }

    await next();
});

app.MapRazorPages();

app.Run();
#elif SQL_Lite
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
#endif