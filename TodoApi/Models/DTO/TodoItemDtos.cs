namespace TodoApi.Models.DTO
{
    public record TodoItemCreateDTO
    {
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public long? TodoListId { get; set; }

    }

    public record TodoItemResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public long? TodoListId { get; set; }
    }

}
