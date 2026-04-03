namespace TodoApi.Models.DTO
{
    public class TodoListResponseDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public long OpenItemsCount { get; set; }
        public long CompletedItemsCount { get; set; }
    }
}
