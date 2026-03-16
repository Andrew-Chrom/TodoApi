using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.Models.DTO;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TodoItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems([FromQuery(Name = "IsCompleted")] bool? IsComplete)
        {
            var UserId = User.FindFirstValue("id");

            if (UserId == null)
                return Unauthorized();

            if (IsComplete == null)
                return await _context.TodoItems.Where(x => x.UserId == UserId).ToListAsync();
            else
                return await _context.TodoItems.Where(x => x.IsComplete == IsComplete && x.UserId == UserId).ToListAsync();
        }

        // GET: api/TodoItems/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var UserId = User.FindFirstValue("id");
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(x => x.Id == id && x.UserId == UserId);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        

        // POST api/toggle/5
        [Authorize]
        [HttpPost("toggle/{id}")]
        public async Task<ActionResult<TodoItem>> ToggleItem(long id)
        {
            var UserId = User.FindFirstValue("id");
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(x => x.Id == id && x.UserId == UserId);

            if (todoItem == null)
                return NotFound();

            todoItem.IsComplete = !todoItem.IsComplete;

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return todoItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemCreateDTO dto)
        {
            var UserId = User.FindFirstValue("id");
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(x => x.Id == id && x.UserId == UserId);

            if (todoItem == null)
                return BadRequest();

            todoItem.Name = dto.Name;
            todoItem.IsComplete = dto.IsComplete;

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItemCreateDTO dto)
        {
            var UserId = User.FindFirstValue("id");
            if (UserId == null)
                return Unauthorized();
            
            var todoItem = new TodoItem
            {
                Name = dto.Name,
                IsComplete = dto.IsComplete,
                UserId = UserId
            };

            _context.TodoItems.Add(todoItem);


            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem   
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var UserId = User.FindFirstValue("id");
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(x => x.Id == id && x.UserId == UserId);
            
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
