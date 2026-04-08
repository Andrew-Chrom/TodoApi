using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApi.Command.TodoList;
using TodoApi.Models;
using TodoApi.Models.DTO;
using TodoApi.Query;
using Wolverine;
using static TodoApi.Command.TodoList.MoveTodoItemHandler;
namespace TodoApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        public IMessageBus _bus;
        public ILogger<TodoListController> _logger;
        public TodoListController(IMessageBus bus, ILogger<TodoListController> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        [HttpGet]
        public async Task<List<TodoListResponseDto>> GetTodoList()
        {
            var userId = User.FindFirstValue("id");
            return await _bus.InvokeAsync<List<TodoListResponseDto>>(new GetTodoListQuery(userId));
        }

        [HttpGet("{id:long}")]
        public async Task<TodoListItemResponseDto> GetTodoListById(long id)
        {
            var userId = User.FindFirstValue("id");
            return await _bus.InvokeAsync<TodoListItemResponseDto>(new GetTodoListByIdQuery(id, userId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoList(TodoListCreateDto dto)
        {
            var userId = User.FindFirstValue("id");
            await _bus.InvokeAsync<long>(new CreateTodoListCommand(dto.Title, userId));
            
            return NoContent();
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateTodoList(long id, TodoListCreateDto dto)
        {
            var userId = User.FindFirstValue("id");
            await _bus.InvokeAsync(new UpdateTodoCommand(id, dto.Title, userId));

            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteTodoList(long id)
        {
            var userId = User.FindFirstValue("id");
            await _bus.InvokeAsync(new DeleteTodoListCommand(id, userId));

            return NoContent();
        }
        [HttpDelete("{todoListId}/items/{todoItemId}")]
        public async Task<IActionResult> RemoveTodoListItem(long todoListId, long todoItemId)
        {
            var userId = User.FindFirstValue("id");
            await _bus.InvokeAsync(new RemoveTodoListCommand(todoListId, todoItemId, userId));

            return NoContent();
        }

        [HttpPost("{todoListId}/items/{todoItemId}")]
        public async Task<IActionResult> AddTodoListItem(long todoListId, long todoItemId)
        {
            var userId = User.FindFirstValue("id");
            await _bus.InvokeAsync(new AddTodoListCommand(todoListId, todoItemId, userId));

            return NoContent();
        }

        [HttpPatch("{todoListId}/items/{destinationTodoListId}")]
        public async Task<IActionResult> MoveTodoListItem([FromRoute(Name = "todoListId")] long todoListId,
            [FromRoute(Name = "destinationTodoListId")] long destinationTodoListId,
            [FromBody] MoveTodoItemDto dto)
        {
            var userId = User.FindFirstValue("id");

            _logger.LogInformation("Moving todo item {TodoItemId} from list {TodoListId} to list {DestinationTodoListId} for user {UserId}",
                dto.TodoItemId, todoListId, destinationTodoListId, userId);

            await _bus.InvokeAsync(new MoveTodoItemCommand(userId, todoListId, dto.TodoItemId, destinationTodoListId));

            return NoContent();
        }

    }
}
