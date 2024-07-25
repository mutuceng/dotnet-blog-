using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Concrete.EfCore;
using Microsoft.AspNetCore.Identity;


namespace Blog.Services
{
    public class RoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService (RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                return IdentityResult.Failed( new IdentityError
                {
                    Description = $"Role {roleName} already exists"
                });
            }   

            AppRole role = new AppRole(roleName);
            return await _roleManager.CreateAsync(role);
        }
    }
}