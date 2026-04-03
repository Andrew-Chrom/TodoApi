using Microsoft.EntityFrameworkCore;
using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.Repositories.TodoItems;

namespace TodoApi.Command.TodoItems
{
    public record DeleteTodo(long Id, string UserId);
    public class DeleteTodoListHandler
    {

        public readonly IWritableTodoItemRepository _repository;
        public DeleteTodoListHandler(IWritableTodoItemRepository repository)
        {
            _repository = repository;
        }
        public async Task Handle(DeleteTodo cmd)
        {
            await _repository.DeleteTodo(cmd.Id, cmd.UserId);
        }

    }
}
