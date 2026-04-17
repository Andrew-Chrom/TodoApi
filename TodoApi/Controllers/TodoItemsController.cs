using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApi.Command.TodoItems;
using TodoApi.Models;
using TodoApi.Models.DTO;
using TodoApi.Query;
using Wolverine;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoItemsController : ControllerBase
    {
        private readonly IMessageBus _bus;
        public TodoItemsController(IMessageBus bus)
        {
            _bus = bus;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemResponseDTO>>> GetTodoItems([FromQuery(Name = "IsCompleted")] bool? IsComplete)
        {
            var userId = User.FindFirstValue("id");

            return await _bus.InvokeAsync<List<TodoItemResponseDTO>>(
                new GetTodosQuery(userId, IsComplete));
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemResponseDTO>> GetTodoItem(long id)
        {
            var userId = User.FindFirstValue("id");
            
            return await _bus.InvokeAsync<TodoItemResponseDTO>(
                new GetTodoById(id, userId));
        }

        // POST api/toggle/5
        [HttpPost("toggle/{id}")]
        public async Task<ActionResult<TodoItem>> ToggleItem(long id)
        {
            var userId = User.FindFirstValue("id");
            
            return await _bus.InvokeAsync<TodoItem>(
                new ToggleTodoCommand(id, userId));
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemCreateDTO dto)
        {
            var userId = User.FindFirstValue("id");
            await _bus.InvokeAsync(new UpdateTodoCommand(id, userId, dto.Name, dto.IsComplete));

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItemCreateDTO dto)
        {
            var userId = User.FindFirstValue("id");
            await _bus.InvokeAsync(new CreateTodoCommand(dto.Name, dto.IsComplete, userId));

            return NoContent();
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var userId = User.FindFirstValue("id");
            await _bus.InvokeAsync(new DeleteTodo(id, userId));

            return NoContent();
        }
    }
}
