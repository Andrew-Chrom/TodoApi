namespace TodoApi.Query.GetTodos;

public record GetTodosQuery(string UserId, bool? IsComplete);
