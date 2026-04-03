namespace TodoApi.Models
{
    public class TodoList
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public List<TodoItem>? Items { get; set; }
        public long OpenItemsCount { get; set; }
        public long CompletedItemsCount { get; set; }
        // Do i need user ID here?
        public string UserId { get; set; }
    }
}
