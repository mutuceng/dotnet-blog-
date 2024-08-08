using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class BestBloggerViewModel
    {
        public string UserId { get; set; } = null!;
        public int PostCount { get; set; }
    }
}