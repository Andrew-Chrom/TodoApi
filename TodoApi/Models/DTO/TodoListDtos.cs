namespace TodoApi.Models.DTO
{
    public record TodoListCreateDto(string Title);

    public record TodoListItemResponseDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public List<TodoItemResponseDTO> Items { get; set; }
        public long OpenItemsCount { get; set; }
        public long CompletedItemsCount { get; set; }
    }

    public record TodoListResponseDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public long OpenItemsCount { get; set; }
        public long CompletedItemsCount { get; set; }
    }

}
