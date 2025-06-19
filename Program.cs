//using Microsoft.EntityFrameworkCore;
//using TaskManagementSystem.Data;
//using TaskManagementSystem.Repositories;
//using TaskManagementSystem.Services;

//var builder = WebApplication.CreateBuilder(args);

//// PostgreSQL Connection
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//// Dependency Injection
//builder.Services.AddScoped<ITaskRepository, TaskRepository>();
//builder.Services.AddScoped<ITaskService, TaskService>();

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Enable Swagger middleware first
//app.UseSwagger();
//app.UseSwaggerUI();

//// Optional: redirect root to /swagger
//app.MapGet("/", context =>
//{
//    context.Response.Redirect("/swagger");
//    return Task.CompletedTask;
//});

//app.UseHttpsRedirection();
//app.UseAuthorization();
//app.MapControllers();

//app.Run();
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TaskManagementSystem.Data;
using TaskManagementSystem.Repositories;
using TaskManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Check and create database if not exists
EnsureDatabaseExists(builder.Configuration);

// ✅ Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Use Swagger first
app.UseSwagger();
app.UseSwaggerUI();

// ✅ Enable CORS middleware
app.UseCors();

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


void EnsureDatabaseExists(IConfiguration config)
{
    var builder = new NpgsqlConnectionStringBuilder(config.GetConnectionString("DefaultConnection"));
    string targetDb = builder.Database;

    // Step 1: Connect to the 'postgres' maintenance DB
    var adminBuilder = new NpgsqlConnectionStringBuilder(builder.ConnectionString)
    {
        Database = "postgres"
    };

    using var adminConn = new NpgsqlConnection(adminBuilder.ConnectionString);
    adminConn.Open();

    using (var checkCmd = adminConn.CreateCommand())
    {
        checkCmd.CommandText = $"SELECT 1 FROM pg_database WHERE datname = '{targetDb}'";
        var exists = checkCmd.ExecuteScalar();

        if (exists == null)
        {
            using var createDbCmd = adminConn.CreateCommand();
            createDbCmd.CommandText = $"CREATE DATABASE \"{targetDb}\"";
            createDbCmd.ExecuteNonQuery();
            Console.WriteLine($"✅ Created database: {targetDb}");
        }
    }

    adminConn.Close();

    // Step 2: Connect to the newly created (or existing) target DB
    using var appConn = new NpgsqlConnection(builder.ConnectionString);
    appConn.Open();

    // Step 3: Create the Tasks table if it doesn't exist
    using var tableCmd = appConn.CreateCommand();
    tableCmd.CommandText = @"
        CREATE TABLE IF NOT EXISTS ""Tasks"" (
            ""TaskId"" SERIAL PRIMARY KEY,
            ""Title"" VARCHAR(255) NOT NULL,
            ""Description"" TEXT NOT NULL,
            ""DueDate"" TIMESTAMPTZ NOT NULL,
            ""Status"" INTEGER NOT NULL
        );
    ";
    tableCmd.ExecuteNonQuery();
    Console.WriteLine("✅ Created table 'Tasks' in database: " + targetDb);
}
