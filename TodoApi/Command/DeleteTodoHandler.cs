using Microsoft.EntityFrameworkCore;
using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Command
{
    public record DeleteTodo(long Id, string UserId);
    public class DeleteTodoHandler
    {

        public readonly IWritableRepository _repository;
        public DeleteTodoHandler(IWritableRepository repository)
        {
            _repository = repository;
        }
        public async Task Handle(DeleteTodo cmd)
        {
            await _repository.DeleteTodo(cmd.Id, cmd.UserId);
        }

    }
}
