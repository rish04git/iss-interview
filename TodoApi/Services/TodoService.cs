using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;
        }

        public void InitializeDatabase() => _repository.InitializeDatabase();

        public Todo CreateTodo(Todo todo)
        {
            var id = _repository.CreateTodo(todo);
            todo.Id = id;
            todo.CreatedAt = DateTime.UtcNow;
            return todo;
        }

        public List<Todo> GetAllTodos() => _repository.GetAllTodos();

        public Todo? GetTodoById(int id) => _repository.GetTodoById(id);

        public Todo? UpdateTodo(int id, Todo todo)
        {
            var ok = _repository.UpdateTodo(id, todo);
            if (!ok) return null;
            todo.Id = id;
            return todo;
        }

        public bool DeleteTodo(int id) => _repository.DeleteTodo(id);
    }
}
