using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class TodoItemsController : BaseController
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
            return await _bus.InvokeAsync<List<TodoItemResponseDTO>>(
                new GetTodosQuery(UserId, IsComplete));
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemResponseDTO>> GetTodoItem([FromRoute] long id)
        {
            return await _bus.InvokeAsync<TodoItemResponseDTO>(
                new GetTodoById(id, UserId));
        }

        // POST api/toggle/5
        [HttpPost("toggle/{id}")]
        public async Task<ActionResult<TodoItem>> ToggleItem([FromRoute] long id)
        {
            return await _bus.InvokeAsync<TodoItem>(
                new ToggleTodoCommand(id, UserId));
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem([FromRoute] long id, [FromBody] UpdateTodoCommand cmd)
        {
            cmd.Id = id;
            cmd.UserId = UserId;
            await _bus.InvokeAsync(cmd);

            return CreatedAtAction(nameof(GetTodoItem), new { id = id }, null);
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem([FromBody] CreateTodoCommand cmd)
        {
            cmd.UserId = UserId;
            var id = await _bus.InvokeAsync<long>(cmd);

            return CreatedAtAction(nameof(GetTodoItem), new { id = id }, null);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem([FromRoute] long id)
        {
            await _bus.InvokeAsync(new DeleteTodo(id, UserId));

            return NoContent();
        }
    }
}
