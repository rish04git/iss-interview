# Solution Overview

## Problems Identified
- Tightly coupled controller and service: controller instantiated TodoService directly, making testing and DI difficult.
- SQL built via string interpolation which is vulnerable to SQL injection and formatting bugs.
- Program.cs performed database initialization inline and duplicated DB path; no configuration.
- Controller used POST for all operations and non-RESTful routes.
- Tests were shallow and relied on implicit global DB file making them brittle.

## Architectural Decisions
- Added ITodoService interface to decouple controller from implementation and enable Dependency injection.
- Refactored TodoService to use parameterized queries (Sqlite parameters) to avoid injection and formatting issues.
- Use dependency injection to provide a single TodoService instance (singleton) configured from app configuration.
- Make controller RESTful using standard HTTP verbs and routes (/api/todos).
- Tests use an isolated temporary SQLite file per test to make tests reliable and independent.

## How to Run
1. Ensure .NET 8 SDK is installed.
2. From solution root run:
   - dotnet run --project TodoApi
3. The API will start and Swagger UI will be available at /swagger when running in Development.

## Running Tests
From solution root run:
- dotnet test

## API Documentation
Base url : /api/todos
- POST /api/todos
  - Body: Todo JSON (Title required)
  - Creates a todo. Returns 201 Created with location header.
	
- GET /api/todos
  - Returns all todos.
	
- GET /api/todos/{id}
  - Returns a single todo by id or 404.
	
- PUT /api/todos/{id}
  - Body: Todo JSON (Title required)
  - Updates a todo. Returns 200 OK or 404 if not found.
	
- DELETE /api/todos/{id}
  - Deletes a todo. Returns 204 No Content or 404 if not found.

Model:
- Id (int)
- Title (string)
- Description (string)
- IsCompleted (bool)
- CreatedAt (DateTime)

## Future Improvements

- Use EF COre for productivity and migrations.
- Add integration tests with test server and in-memory DB.
- Add validation attributes for clearer model contracts.
- Add paging/filtering endpoints and search.
