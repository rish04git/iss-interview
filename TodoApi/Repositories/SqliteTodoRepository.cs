using Microsoft.Data.Sqlite;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    internal static class Sql
    {
        public const string CreateTable = @"CREATE TABLE IF NOT EXISTS Todos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    IsCompleted INTEGER NOT NULL DEFAULT 0,
                    CreatedAt TEXT NOT NULL
        )";

        public const string InsertTodo = "INSERT INTO Todos (Title, Description, IsCompleted, CreatedAt) VALUES ($title, $description, $isCompleted, $createdAt); SELECT last_insert_rowid();";
        public const string SelectAll = "SELECT Id, Title, Description, IsCompleted, CreatedAt FROM Todos";
        public const string SelectById = "SELECT Id, Title, Description, IsCompleted, CreatedAt FROM Todos WHERE Id = $id";
        public const string UpdateById = "UPDATE Todos SET Title = $title, Description = $description, IsCompleted = $isCompleted WHERE Id = $id";
        public const string DeleteById = "DELETE FROM Todos WHERE Id = $id";
    }

    public class SqliteTodoRepository : ITodoRepository
    {
        private readonly string _connectionString;

        public SqliteTodoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = Sql.CreateTable;
            cmd.ExecuteNonQuery();
        }

        public int CreateTodo(Todo todo)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = Sql.InsertTodo;
            cmd.Parameters.AddWithValue("$title", todo.Title ?? string.Empty);
            cmd.Parameters.AddWithValue("$description", todo.Description ?? string.Empty);
            cmd.Parameters.AddWithValue("$isCompleted", todo.IsCompleted ? 1 : 0);
            cmd.Parameters.AddWithValue("$createdAt", DateTime.UtcNow.ToString("o"));

            var id = Convert.ToInt32(cmd.ExecuteScalar());
            return id;
        }

        public List<Todo> GetAllTodos()
        {
            var todos = new List<Todo>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = Sql.SelectAll;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                todos.Add(new Todo
                {
                    Id = reader.GetInt32(0),
                    Title = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    IsCompleted = reader.GetInt32(3) == 1,
                    CreatedAt = DateTime.Parse(reader.GetString(4))
                });
            }
            return todos;
        }

        public Todo? GetTodoById(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = Sql.SelectById;
            cmd.Parameters.AddWithValue("$id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Todo
                {
                    Id = reader.GetInt32(0),
                    Title = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    IsCompleted = reader.GetInt32(3) == 1,
                    CreatedAt = DateTime.Parse(reader.GetString(4))
                };
            }
            return null;
        }

        public bool UpdateTodo(int id, Todo todo)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = Sql.UpdateById;
            cmd.Parameters.AddWithValue("$title", todo.Title ?? string.Empty);
            cmd.Parameters.AddWithValue("$description", todo.Description ?? string.Empty);
            cmd.Parameters.AddWithValue("$isCompleted", todo.IsCompleted ? 1 : 0);
            cmd.Parameters.AddWithValue("$id", id);

            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        public bool DeleteTodo(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = Sql.DeleteById;
            cmd.Parameters.AddWithValue("$id", id);

            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }
    }
}
