# Dotnet Razor Pages Movie App

This is a sample ASP.NET Core Razor Pages application that demonstrates how to use Razor Pages to create dynamic, data-driven web applications.

## Features

- **ASP.NET Core:** A cross-platform framework for building modern cloud-based applications.
- **Razor Pages:** A page-focused framework for building dynamic web applications.
- **Entity Framework Core:** An ORM for data access.

## Getting Started

### Prerequisites

- .NET Core SDK (version 3.1 or later)
- SQL Server (local or remote)

### Installation

1. Clone or fork the repository.
2. Restore dependencies.
3. Update the database.

Command examples:

To run a local dev MSSQL database in a docker container image:

```bash
# Start the MSSQL container
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourStrong!Passw0rd' -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server

# Wait for the SQL Server to start
sleep 20

# Execute the SQL script to create the database
docker exec -i sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'YourStrong!Passw0rd' -Q "CREATE DATABASE RazorPagesMovieContext;"
```

> **Friendly Reminder:** The command above is intended for local development and testing environments only. For production, it's highly recommended to use a managed SQL Server instance. If you're encountering SSL certificate verification errors and don't plan to use SSL, you can bypass this by adding the `-C` option to the `sqlcmd` command. This option tells the client to trust the server's certificate without validation.



dotnet commands to pull dependencies, build binaries and run database migrations:
```bash
dotnet restore RazorPagesMovie.csproj
dotnet build RazorPagesMovie.csproj
dotnet ef database update
```

### Running the Application

To run the application, use the appropriate .NET Core commands. 
Example:
```bash
dotnet run
```

The application should be locally available at `https://localhost:5001`.

## Project Structure

- **Pages Folder:** Contains Razor pages (.cshtml files) and their corresponding page models (.cshtml.cs files).
- **wwwroot Folder:** Static files like CSS, JavaScript, and images.
- **Startup.cs:** Configures the application, services, and request pipeline.
- **Program.cs:** Entry point for the application.

## Dependencies

Since this is an ASP.NET Core project, the primary dependencies are:

- **Microsoft.AspNetCore.App:** This meta-package includes all the necessary packages for building ASP.NET Core applications.
- **Microsoft.EntityFrameworkCore.SqlServer:** Package for using Entity Framework Core with SQL Server.

## Source

This project is borrowed from [this source example](https://github.com/dotnet/AspNetCore.Docs/tree/main/aspnetcore/tutorials/razor-pages/razor-pages-start/sample/RazorPagesMovie90).

## Language Composition

- **C#:** 60.2%
- **HTML:** 35.7%
- **CSS:** 3.5%
- **JavaScript:** 0.6%
