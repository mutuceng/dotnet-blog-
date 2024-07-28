using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Blog.Entity
{
    public class User:IdentityUser
    {
        public string? FullName {get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? ProfilePhoto { get; set; }
        public string RoleName { get; set; } = "User";
        
        // Blogger 
        public string? LinkedInProfile { get; set; }
        public IFormFile? ResumeFile { get; set; }    

        public ICollection<PostInteraction> PostInteractions { get; set; }= new List<PostInteraction>();
        public ICollection<Post> Posts { get; set; }= new List<Post>();
        public ICollection<Comment> Comments { get; set; }= new List<Comment>();
        public Role Role {get; set;} = null!;

        
        
    }

        
}