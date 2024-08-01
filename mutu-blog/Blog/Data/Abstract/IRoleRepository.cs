using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entity;

namespace Blog.Data.Abstract
{
    public interface IRoleRepository
    {
        IQueryable<Role> Roles { get; }
        void CreateRole(Role role);
    }
}