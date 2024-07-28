using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Blog.Entity
{
    public class Role: IdentityRole
    {
        
      public User? User { get; set; } 
    }
}