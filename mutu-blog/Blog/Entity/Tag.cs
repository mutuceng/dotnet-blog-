using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Entity
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}