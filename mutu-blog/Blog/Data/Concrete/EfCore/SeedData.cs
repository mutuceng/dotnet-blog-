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
                        RoleId = "93416040-9c60-47c2-9ebb-0eb9eaf33c95"
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
                
                if(!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User { 
                            UserName = "mutu", 
                            FullName = "Umut Tanrıverdi", 
                            Email = "umut.tanriverdiceng@gmail.com", 
                            LinkedInProfile = "https://www.linkedin.com/in/umut-tanrıverdi-2035b8259/",
                            ProfilePhoto = "~/images/profile.jpeg", RoleId = "5edb0895-588d-4064-8307-758bdd5fb8f5"},
                        new User { 
                            UserName = "umut",  
                            Email = "umut.tanriverdi5012@gmail.com", 
                            ProfilePhoto = "~/images/blank-profile-pic.jpg", RoleId = "85512be7-2d8e-46aa-ae95-34a333e836de"}
                    );
                    context.SaveChanges();
                }

                if(!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { Name = "web development", TagId = 4725},
                        new Tag { Name = "backend", TagId=4726},
                        new Tag { Name = "frontend", TagId=4727},
                        new Tag { Name = "dotnet", TagId=4728 },
                        new Tag { Name = "html-css", TagId=4729}
                    );
                    context.SaveChanges();
                }

                
                if(!context.Posts.Any())
                {
                    context.Posts.AddRange(
                        new Post {
                            Title = "Asp.net core",
                            Content = "Ut mollitia non voluptates aliquam enim. Et minus adipisci consequuntur corporis et rerum velit ea. Animi at id id voluptas harum",
                            PostId = 6000,
                            IsActive = true,
                            CreatedDate = DateTime.Now.AddDays(-10),
                            Tags = context.Tags.Take(3).ToList(),
                            PrimaryTagId = 4726,
                            PostImage = "~/images/DSC_6795.jpg",
                            UserId = "e38415bf-ef4c-4a65-b104-5ce3bff8dc24",
                        },
                        new Post {
                            Title = "Php",
                            Content = "Ut mollitia non voluptates aliquam enim. Et minus adipisci consequuntur corporis et rerum velit ea. Animi at id id voluptas harum",
                            PostId = 6001,
                            IsActive = true,
                            PostImage = "~/wwwroot/images/DSC_6795.jpg",
                            CreatedDate = DateTime.Now.AddDays(-20),
                            Tags = context.Tags.Take(2).ToList(),
                            PrimaryTagId = 4726,
                            UserId = "e38415bf-ef4c-4a65-b104-5ce3bff8dc24"
                        },
                        new Post {
                            Title = "Django",
                            Content = "Ut mollitia non voluptates aliquam enim. Et minus adipisci consequuntur corporis et rerum velit ea. Animi at id id voluptas harum",
                            PostId = 6002,
                            IsActive = true,
                            PostImage = "~/images/DSC_6795.jpg",
                            CreatedDate = DateTime.Now.AddDays(-30),
                            Tags = context.Tags.Take(4).ToList(),
                            PrimaryTagId = 4726,
                            UserId = "e38415bf-ef4c-4a65-b104-5ce3bff8dc24"
                        }
                        ,
                        new Post {
                            Title = "React Dersleri",
                            Content = "Ut mollitia non voluptates aliquam enim. Et minus adipisci consequuntur corporis et rerum velit ea. Animi at id id voluptas harum",
                            PostId = 6003,
                            IsActive = true,
                            PostImage = "~/images/DSC_6795.jpg",
                            CreatedDate = DateTime.Now.AddDays(-40),
                            Tags = context.Tags.Take(4).ToList(),
                            PrimaryTagId = 4727,
                            UserId = "e38415bf-ef4c-4a65-b104-5ce3bff8dc24"
                        }
                        ,
                        new Post {
                            Title = "Angular",
                            Content = "Ut mollitia non voluptates aliquam enim. Et minus adipisci consequuntur corporis et rerum velit ea. Animi at id id voluptas harum",
                            PostId = 6004,
                            IsActive = true,
                            PostImage = "~/images/DSC_6795.jpg",
                            CreatedDate = DateTime.Now.AddDays(-50),
                            Tags = context.Tags.Take(4).ToList(),
                            PrimaryTagId = 4727,
                            UserId = "e38415bf-ef4c-4a65-b104-5ce3bff8dc24"
                        }
                        ,
                        new Post {
                            Title = "Web Tasarım",
                            Content = "Ut mollitia non voluptates aliquam enim. Et minus adipisci consequuntur corporis et rerum velit ea. Animi at id id voluptas harum",
                            PostId = 6005,
                            IsActive = true,
                            PostImage = "~/images/DSC_6795.jpg",
                            CreatedDate = DateTime.Now.AddDays(-60),
                            Tags = context.Tags.Take(4).ToList(),
                            PrimaryTagId = 4725,
                            UserId = "e38415bf-ef4c-4a65-b104-5ce3bff8dc24"
                        }
                    );
                    context.SaveChanges();
                }




            }
        }
    }
}