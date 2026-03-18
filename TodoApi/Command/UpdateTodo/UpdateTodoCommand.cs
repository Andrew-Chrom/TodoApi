using System.Xml.Linq;

namespace TodoApi.Command.UpdateTodo;

public record UpdateTodoCommand(long Id, string UserId, string Name, bool IsComplete);
