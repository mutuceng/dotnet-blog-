using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Entity
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; }
        public int UserId {get; set;}
        public int PrimaryTagId {get; set;}
        public User User {get; set;} = null!;
        public PrimaryTag PrimaryTag {get; set;} = null!;
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<Comment> Comments { get; set; }= new List<Comment>();
        public ICollection<PostInteraction> PostInteractions { get; set; }= new List<PostInteraction>();

    }
}