using Microsoft.Data.Sqlite;
using TodoApi.Repositories;
using TodoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection string
var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:Default") ?? "Data Source=todos.db";

// Register repository and service
builder.Services.AddSingleton<ITodoRepository>(sp => new SqliteTodoRepository(connectionString));
builder.Services.AddSingleton<ITodoService>(sp => new TodoService(sp.GetRequiredService<ITodoRepository>()));

var app = builder.Build();

// Initialize DB
var repo = app.Services.GetRequiredService<ITodoRepository>();
repo.InitializeDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
