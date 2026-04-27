using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Command.TodoList;
using TodoApi.Models.DTO;
using TodoApi.Query;
using Wolverine;
using static TodoApi.Command.TodoList.MoveTodoItemHandler;

namespace TodoApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListController : BaseController
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
            return await _bus.InvokeAsync<List<TodoListResponseDto>>(new GetTodoListQuery(UserId));
        }

        [HttpGet("{id:long}")]
        public async Task<TodoListItemResponseDto> GetTodoListById([FromRoute] long id)
        {
            return await _bus.InvokeAsync<TodoListItemResponseDto>(new GetTodoListByIdQuery(id, UserId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoList([FromBody] CreateTodoListCommand cmd)
        { 
            cmd.UserId = UserId;
            await _bus.InvokeAsync<long>(cmd);
            
            return NoContent();
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateTodoList([FromRoute] long id, [FromBody] UpdateTodoCommand cmd)
        {
            cmd.Id = id;
            cmd.UserId = UserId;
            await _bus.InvokeAsync(cmd);

            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteTodoList([FromRoute] long id)
        {   
            await _bus.InvokeAsync(new DeleteTodoListCommand(id, UserId));

            return NoContent();
        }
        [HttpDelete("{todoListId}/items/{todoItemId}")]
        public async Task<IActionResult> RemoveTodoListItem(long todoListId, long todoItemId)
        { 
            await _bus.InvokeAsync(new RemoveTodoListCommand(todoListId, todoItemId, UserId));

            return NoContent();
        }

        [HttpPost("{todoListId}/items/{todoItemId}")]
        public async Task<IActionResult> AddTodoListItem(long todoListId, long todoItemId)
        {    
            await _bus.InvokeAsync(new AddTodoListCommand(todoListId, todoItemId, UserId));

            return NoContent();
        }

        [HttpPatch("{todoListId}/items/{destinationTodoListId}")]
        public async Task<IActionResult> MoveTodoListItem([FromRoute(Name = "todoListId")] long todoListId,
            [FromRoute(Name = "destinationTodoListId")] long destinationTodoListId,
            [FromBody] MoveTodoItemCommand cmd)
        {
            _logger.LogInformation("Moving todo item {TodoItemId} from list {TodoListId} to list {DestinationTodoListId} for user {UserId}",
                cmd.TodoItemId, todoListId, destinationTodoListId, UserId);
            
            cmd.UserId = UserId;
            cmd.TodoListId = todoListId;
            cmd.DestinationTodoListId = destinationTodoListId;

            await _bus.InvokeAsync(cmd);

            return NoContent();
        }

    }
}
