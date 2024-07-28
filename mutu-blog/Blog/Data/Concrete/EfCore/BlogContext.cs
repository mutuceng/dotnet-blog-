using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Concrete.EfCore
{
    public class BlogContext:  IdentityDbContext<User, Role, string>
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {

        }
        public DbSet<Post> Posts => Set<Post>();  
        public DbSet<Tag> Tags => Set<Tag>(); 
        public DbSet<Comment> Comments => Set<Comment>(); 
        public DbSet<PrimaryTag> PrimaryTags => Set<PrimaryTag>(); 
        public DbSet<PostInteraction> PostInteractions => Set<PostInteraction>(); 
    }
}