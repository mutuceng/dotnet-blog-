using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Entity
{
    public class Tag:BaseTag
    {
        public int TagId { get; set; }
        public ICollection<Post>? Posts { get; set; }
    }
}