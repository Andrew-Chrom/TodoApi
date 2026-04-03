using Microsoft.EntityFrameworkCore;
using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.Repositories;
using TodoApi.Repositories.TodoLists;

namespace TodoApi.Command.TodoList
{
    public record DeleteTodoListCommand(long Id, string UserId);
    public class DeleteTodoListHandler
    {

        public readonly IWritableTodoListRepository _repository;
        public DeleteTodoListHandler(IWritableTodoListRepository repository)
        {
            _repository = repository;
        }
        public async Task Handle(DeleteTodoListCommand cmd)
        {
            await _repository.DeleteTodoList(cmd.Id, cmd.UserId);
        }

    }
}
