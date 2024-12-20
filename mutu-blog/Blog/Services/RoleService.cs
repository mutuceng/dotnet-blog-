using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entity;
using Microsoft.AspNetCore.Identity;


namespace Blog.Services
{
    public class RoleService
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleService (RoleManager<Role> roleManager)
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

            Role role = new Role{ Name = roleName };
            return await _roleManager.CreateAsync(role);
        }
    }
}