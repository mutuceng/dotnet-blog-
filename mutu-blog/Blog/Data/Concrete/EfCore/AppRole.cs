using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Blog.Data.Concrete.EfCore
{
    public class AppRole : IdentityRole
    {
        public AppRole(string roleName) : base(roleName)
        {
        }
    }
}