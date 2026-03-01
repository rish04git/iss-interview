using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpPost]
        public IActionResult CreateTodo([FromBody] Todo todo)
        {
            if (todo == null || string.IsNullOrWhiteSpace(todo.Title))
                return BadRequest("Title is required");

            var result = _todoService.CreateTodo(todo);
            return CreatedAtAction(nameof(GetTodoById), new { id = result.Id }, result);
        }

        [HttpGet]
        public IActionResult GetAllTodos()
        {
            var todos = _todoService.GetAllTodos();
            return Ok(todos);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetTodoById(int id)
        {
            var todo = _todoService.GetTodoById(id);
            if (todo == null) return NotFound();
            return Ok(todo);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateTodo(int id, [FromBody] Todo todo)
        {
            if (todo == null || string.IsNullOrWhiteSpace(todo.Title))
                return BadRequest("Title is required");

            var updated = _todoService.UpdateTodo(id, todo);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteTodo(int id)
        {
            var deleted = _todoService.DeleteTodo(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
