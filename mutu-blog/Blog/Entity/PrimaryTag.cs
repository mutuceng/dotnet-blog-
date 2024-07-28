using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Entity
{
    public class PrimaryTag:Tag
    {
        public ICollection<Post> PrimaryPosts { get; set; } = new List<Post>();
    }
}