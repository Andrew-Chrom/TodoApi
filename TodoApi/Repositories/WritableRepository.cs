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

namespace TodoApi.Repositories
{
    public class WritableRepository : IWritableRepository
    {
        public readonly CommandDbContext _ctx;
        public WritableRepository(CommandDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<long> CreateTodo(TodoItem item)
        {
            //var todo = new TodoItem(item);
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

            //_ctx.Entry(todoItem).State = EntityState.Modified;

            await _ctx.SaveChangesAsync();
        }
        public async Task DeleteTodo(long id, string userId)
        {
            var todoItem = await _ctx.TodoItems.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (todoItem == null)
                throw new NotFoundException("Todo not found");

            _ctx.TodoItems.Remove(todoItem);
            await _ctx.SaveChangesAsync();
        }
    }
}
