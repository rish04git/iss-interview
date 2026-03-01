using Xunit;
using TodoApi.Services;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Tests;

public class TodoApiTests
{
    private ITodoService CreateServiceWithTempDb()
    {
        var tempFile = Path.GetTempFileName();
        var connectionString = $"Data Source={tempFile}";
        var repo = new SqliteTodoRepository(connectionString);
        repo.InitializeDatabase();
        var service = new TodoService(repo);
        return service;
    }

    [Fact]
    public void CreateTodo_Succeeds()
    {
        var service = CreateServiceWithTempDb();
        var todo = new Todo { Title = "Test", Description = "Desc" };
        var result = service.CreateTodo(todo);
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
    }

    [Fact]
    public void GetAllTodos_ReturnsCreated()
    {
        var service = CreateServiceWithTempDb();
        service.CreateTodo(new Todo { Title = "Item1" });
        service.CreateTodo(new Todo { Title = "Item2" });

        var all = service.GetAllTodos();
        Assert.Equal(2, all.Count);
    }

    [Fact]
    public void GetById_ReturnsCorrect()
    {
        var service = CreateServiceWithTempDb();
        var created = service.CreateTodo(new Todo { Title = "X" });

        var fetched = service.GetTodoById(created.Id);
        Assert.NotNull(fetched);
        Assert.Equal(created.Title, fetched!.Title);
    }

    [Fact]
    public void Update_ReturnsNullForMissing()
    {
        var service = CreateServiceWithTempDb();
        var updated = service.UpdateTodo(999, new Todo { Title = "No" });
        Assert.Null(updated);
    }

    [Fact]
    public void Update_Succeeds()
    {
        var service = CreateServiceWithTempDb();
        var created = service.CreateTodo(new Todo { Title = "ToUpdate" });
        var updated = service.UpdateTodo(created.Id, new Todo { Title = "Updated", Description = "D" });
        Assert.NotNull(updated);
        Assert.Equal(created.Id, updated!.Id);
        Assert.Equal("Updated", updated.Title);
    }

    [Fact]
    public void Delete_ReturnsFalseWhenMissing()
    {
        var service = CreateServiceWithTempDb();
        var result = service.DeleteTodo(999);
        Assert.False(result);
    }

    [Fact]
    public void Delete_Succeeds()
    {
        var service = CreateServiceWithTempDb();
        var created = service.CreateTodo(new Todo { Title = "ToDelete" });
        var deleted = service.DeleteTodo(created.Id);
        Assert.True(deleted);
        var fetched = service.GetTodoById(created.Id);
        Assert.Null(fetched);
    }
}
