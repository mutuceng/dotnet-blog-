using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Entity
{
    public class PostInteraction
    {
        public int UserId {get; set;}
        public int PostId { get; set; }
        public int ViewCount { get; set; } = 0;
        public TimeSpan TimeSpent { get; set; } = TimeSpan.Zero;

        public Post Post { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}