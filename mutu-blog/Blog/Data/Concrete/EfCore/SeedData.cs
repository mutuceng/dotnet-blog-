using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Concrete.EfCore
{
    public class SeedData
    {
        private const string adminUser ="Admin";
        private const string adminPassword = "admin123D!";

        public static async void TestUser(IApplicationBuilder app)
        {
            // var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<User>();

            using ( var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BlogContext>();

                if(context.Database.GetAppliedMigrations().Any())
                {
                    context.Database.Migrate();
                }
                
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                // var roleManager = scope.ServiceProvider.GetRequiredService<Role>();

                var user = await userManager.FindByNameAsync(adminUser);  

                if (user == null)
                {
                    user = new User {
                        UserName = adminUser,
                        Email = "admin@mutu.com",
                        RoleId = "db9b3631-dd65-49ae-8411-401a95ff0e3a"
                    };

                    var result = await userManager.CreateAsync(user, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        // Hataları loglayın veya kontrol edin
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"Error: {error.Description}");
                        }
                    }
                }
            }
        }
    }
}