using Blog.Data.Abstract;
using Blog.Entity;

namespace Blog.Models
{
    public class BlogDetailsViewModel
    {
        public Post Blog { get; set; } 
        public List<Comment> Comments { get; set; }
        public User Blogger {get; set;}
    }
}
