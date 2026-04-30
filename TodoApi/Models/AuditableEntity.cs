namespace TodoApi.Models
{
    public abstract class AuditableEntity
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
