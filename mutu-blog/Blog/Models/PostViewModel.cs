using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entity;

namespace Blog.Models
{
    public class PostViewModel
    {
        public List<Post> Posts { get; set; } = new();
    }
}