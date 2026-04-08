using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    public class TodoList
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public ICollection<TodoItem>? Items { get; set; } = new List<TodoItem>();
        public long OpenItemsCount { get; set; }
        public long CompletedItemsCount { get; set; }
        
        [Column("user_id")]
        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
