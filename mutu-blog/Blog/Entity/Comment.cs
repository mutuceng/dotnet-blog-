using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Entity
{
    public class Comment
    {
        public int Id {get; set;}
        public string Text { get; set; } = null!;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int LikeNumber { get; set; } = 0;
        public int DisslikeNumber { get; set; } = 0;
        public Post Post {get; set;} = null!;
        public int PostId { get; set; }
        public User User {get; set;} = null!;
        public string UserId { get; set; } = null!;
    }
}