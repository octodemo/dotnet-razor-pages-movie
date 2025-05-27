#define DEFAULT // SQL server is default, SQL_Lite is other
#if DEFAULT
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AzureStorage;

var builder = WebApplication.CreateBuilder(args);

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddRazorPages();
var disableSession = Environment.GetEnvironmentVariable("DISABLE_SESSION");
var sessionEnabled = string.IsNullOrEmpty(disableSession) || disableSession.ToLower() != "true";
if (sessionEnabled)
{
    // Configure Data Protection to persist keys to Azure Blob Storage if connection string and container are set
    var blobConnStr = Environment.GetEnvironmentVariable("AZURE_BLOB_KEYRING_CONNECTION_STRING");
    var blobContainer = Environment.GetEnvironmentVariable("AZURE_BLOB_KEYRING_CONTAINER");
    var blobName = Environment.GetEnvironmentVariable("AZURE_BLOB_KEYRING_BLOB") ?? "dataprotection-keys.xml";
    if (!string.IsNullOrEmpty(blobConnStr) && !string.IsNullOrEmpty(blobContainer))
    {
        builder.Services.AddDataProtection()
            .PersistKeysToAzureBlobStorage(blobConnStr, blobContainer, blobName)
            .SetApplicationName("RazorPagesMovie");
    }
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });
}
builder.Services.AddDbContext<RazorPagesMovieContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RazorPagesMovieContext") ?? throw new InvalidOperationException("Connection string 'RazorPagesMovieContext' not found.")));

var app = builder.Build();

// Update database initialization with error handling
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        var context = services.GetRequiredService<RazorPagesMovieContext>();
        
        logger.LogInformation("Starting database migration");
        context.Database.Migrate();
        
        logger.LogInformation("Starting data seeding");
        SeedData.Initialize(services);
        
        logger.LogInformation("Database initialization completed");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating/seeding the database.");
        throw;
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
if (sessionEnabled)
{
    app.UseSession();
}
app.Use(async (context, next) =>
{
    var path = context.Request.Path;
    var isAccountPage = path.StartsWithSegments("/Account");
    var isAuthenticated = false;
    if (sessionEnabled)
    {
        // Defensive: Only check session if session cookie exists
        if (context.Request.Cookies.ContainsKey(".AspNetCore.Session"))
        {
            isAuthenticated = context.Session.GetInt32("UserId").HasValue;
        }
        else
        {
            // No session cookie, treat as not authenticated
            isAuthenticated = false;
        }
    }
    else
    {
        // If session is disabled, treat all users as authenticated to avoid redirect loops
        isAuthenticated = true;
    }
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
using Microsoft.AspNetCore.DataProtection.AzureStorage;

var builder = WebApplication.CreateBuilder(args);

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddRazorPages();
var disableSession = Environment.GetEnvironmentVariable("DISABLE_SESSION");
var sessionEnabled = string.IsNullOrEmpty(disableSession) || disableSession.ToLower() != "true";
if (sessionEnabled)
{
    // Configure Data Protection to persist keys to Azure Blob Storage if connection string and container are set
    var blobConnStr = Environment.GetEnvironmentVariable("AZURE_BLOB_KEYRING_CONNECTION_STRING");
    var blobContainer = Environment.GetEnvironmentVariable("AZURE_BLOB_KEYRING_CONTAINER");
    var blobName = Environment.GetEnvironmentVariable("AZURE_BLOB_KEYRING_BLOB") ?? "dataprotection-keys.xml";
    if (!string.IsNullOrEmpty(blobConnStr) && !string.IsNullOrEmpty(blobContainer))
    {
        builder.Services.AddDataProtection()
            .PersistKeysToAzureBlobStorage(blobConnStr, blobContainer, blobName)
            .SetApplicationName("RazorPagesMovie");
    }
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });
}
builder.Services.AddDbContext<RazorPagesMovieContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("RazorPagesMovieContext") ?? throw new InvalidOperationException("Connection string 'RazorPagesMovieContext' not found.")));

var app = builder.Build();

// Update database initialization with error handling
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        var context = services.GetRequiredService<RazorPagesMovieContext>();
        
        logger.LogInformation("Starting database migration");
        context.Database.Migrate();
        
        logger.LogInformation("Starting data seeding");
        SeedData.Initialize(services);
        
        logger.LogInformation("Database initialization completed");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating/seeding the database.");
        throw;
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
if (sessionEnabled)
{
    app.UseSession();
}
app.Use(async (context, next) =>
{
    var path = context.Request.Path;
    var isAccountPage = path.StartsWithSegments("/Account");
    var isAuthenticated = false;
    if (sessionEnabled)
    {
        // Defensive: Only check session if session cookie exists
        if (context.Request.Cookies.ContainsKey(".AspNetCore.Session"))
        {
            isAuthenticated = context.Session.GetInt32("UserId").HasValue;
        }
        else
        {
            // No session cookie, treat as not authenticated
            isAuthenticated = false;
        }
    }
    else
    {
        // If session is disabled, treat all users as authenticated to avoid redirect loops
        isAuthenticated = true;
    }
    if (!isAuthenticated && !isAccountPage && !path.StartsWithSegments("/Index"))
    {
        context.Response.Redirect("/Account/Login");
        return;
    }

    await next();
});
app.MapRazorPages();

app.Run();
#endif