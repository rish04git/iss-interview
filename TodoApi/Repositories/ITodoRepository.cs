using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface ITodoRepository
    {
        void InitializeDatabase();
        int CreateTodo(Todo todo);
        List<Todo> GetAllTodos();
        Todo? GetTodoById(int id);
        bool UpdateTodo(int id, Todo todo);
        bool DeleteTodo(int id);
    }
}
