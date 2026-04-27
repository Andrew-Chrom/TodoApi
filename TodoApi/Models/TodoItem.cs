using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    public class TodoItem : AuditableEntity
    {
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long? TodoListId { get; set; }

        [ForeignKey(nameof(TodoListId))]
        public TodoList? TodoList { get; set; }
    }
}
