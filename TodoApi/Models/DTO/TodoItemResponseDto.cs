namespace TodoApi.Models.DTO
{
    public class TodoItemResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public long? TodoListId { get; set; }
    }
}
