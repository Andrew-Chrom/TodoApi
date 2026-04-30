using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    public class TodoList : AuditableEntity
    {
        public string Title { get; set; }
        public ICollection<TodoItem> Items { get; set; } = new List<TodoItem>();
        public long OpenItemsCount { get; set; }
        public long CompletedItemsCount { get; set; }
        
        [Column("user_id")]
        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
