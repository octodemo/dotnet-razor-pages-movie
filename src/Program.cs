#define DEFAULT // SQL server is default, SQL_Lite is other
#if DEFAULT
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddRazorPages()
    .AddMvcOptions(options => options.Filters.Add(new IgnoreAntiforgeryTokenAttribute()));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddDbContext<RazorPagesMovieContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RazorPagesMovieContext") ?? throw new InvalidOperationException("Connection string 'RazorPagesMovieContext' not found.")));

// Configure Data Protection to use ephemeral keys
builder.Services.AddDataProtection()
    .SetApplicationName("RazorPagesMovie")
    .UseEphemeralDataProtectionProvider();

// Disable anti-forgery token validation globally for controllers/views (if needed)
builder.Services.AddControllersWithViews(options =>
    options.Filters.Add(new IgnoreAntiforgeryTokenAttribute()));

var app = builder.Build();

// Update database initialization with error handling
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<RazorPagesMovieContext>();
        context.Database.Migrate();
        SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating/seeding the database.");
        throw;
    }
}

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