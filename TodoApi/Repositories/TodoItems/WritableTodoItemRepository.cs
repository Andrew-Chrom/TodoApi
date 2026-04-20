using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TodoApi.Errors;
using TodoApi.Models;
using TodoApi.Models.DTO;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApi.Command;
using TodoApi.Query;
using Wolverine;

namespace TodoApi.Repositories.TodoItems
{
    public class WritableTodoItemRepository : IWritableTodoItemRepository
    {
        public readonly CommandDbContext _ctx;
        
        public WritableTodoItemRepository(CommandDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<TodoItemResponseDTO> GetTodoById(long id, string userId)
        {
            var todoItem = await _ctx.TodoItems.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            return new TodoItemResponseDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete,
                TodoListId = todoItem.TodoListId,
            };

        }
        public async Task<long> CreateTodo(TodoItem item)
        {
            _ctx.TodoItems.Add(item);
            await _ctx.SaveChangesAsync();
            
            return item.Id;
        }
        public async Task UpdateTodo(TodoItemCreateDTO dto, long id, string userId)
        {

            var todoItem = await _ctx.TodoItems.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (todoItem == null)
                throw new NotFoundException("Todo not found");

            todoItem.Name = dto.Name;
            todoItem.IsComplete = dto.IsComplete;
            todoItem.TodoListId = dto.TodoListId;
        }
        public async Task DeleteTodo(long id, string userId)
        {
            var todoItem = await _ctx.TodoItems.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (todoItem == null)
                throw new NotFoundException("Todo not found");

            _ctx.TodoItems.Remove(todoItem);
        }
    }
}
