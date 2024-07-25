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
        public string UserType { get; set; } = "User";
        public string? ProfilePhoto { get; set; }
    }
}