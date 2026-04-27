using Microsoft.AspNetCore.Identity;

namespace TodoApi.Models
{
    public class User : IdentityUser
    {
        public User(string email) : base(email) => Email = email;
        public virtual ICollection<TodoList> TodoLists { get; set; } = new List<TodoList>();
    }
}
