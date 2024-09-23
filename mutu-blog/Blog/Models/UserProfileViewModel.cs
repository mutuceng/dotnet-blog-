using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entity;

namespace Blog.Models
{
    public class UserProfileViewModel
    {
        public List<Post> Blogs { get; set; } 
        public List<Comment> Comments { get; set; }
        public User Blogger {get; set;}
    }
}