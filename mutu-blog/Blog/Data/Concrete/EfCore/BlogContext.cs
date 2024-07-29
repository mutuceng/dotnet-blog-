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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);
            
            builder.Entity<User>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);

            builder.Entity<User>()
                .HasMany( u => u.PostInteractions)
                .WithOne( pi => pi.User)
                .HasForeignKey(pi => pi.UserId);

            builder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany( r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);    
;
        
            builder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId);
            
            // Many-to-Many relationship for Post and Tags
            builder.Entity<Post>()
                .HasMany(p => p.Tags)
                .WithMany(t => t.Posts)  // Posts propertysi olmalı
                .UsingEntity<Dictionary<string, object>>(
                    "PostTag",
                    j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                    j => j.HasOne<Post>().WithMany().HasForeignKey("PostId"));

            builder.Entity<Post>()
                .HasMany(p => p.PostInteractions)
                .WithOne(pi => pi.Post)
                .HasForeignKey(pi => pi.PostId);    

            // One-to-Many relationship for Post and PrimaryTag
            builder.Entity<Post>()
                .HasOne(p => p.PrimaryTag)
                .WithMany(pt => pt.PrimaryPosts)
                .HasForeignKey(p => p.PrimaryTagId)
                .OnDelete(DeleteBehavior.Restrict);    

            builder.Entity<Post>()
                .HasMany( p => p.PostInteractions)
                .WithOne( pi => pi.Post)
                .HasForeignKey(pi => pi.PostId);   
            
            

        }


    }
}