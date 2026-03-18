namespace TodoApi.Command.CreateTodo;

public record CreateTodoCommand(string Name, bool IsComplete, string UserId);
