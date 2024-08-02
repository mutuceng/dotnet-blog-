using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.ViewComponents
{
    public class PopularPosts: ViewComponent
    {
        private IPostRepository _postRepository;
        public PopularPosts(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var posts = await _postRepository
                                    .Posts
                                    .Select(post => new 
                                    {
                                        Post = post,
                                        PopularityScore = post.PostInteractions.Sum(pi => pi.ViewCount + pi.TimeSpent.TotalSeconds/1000)
                                    })
                                    .OrderByDescending(x => x.PopularityScore)
                                    .Take(6)
                                    .Select(x => x.Post)
                                    .ToListAsync();

                if (!posts.Any())
                {
                    return View("Error", "No popular posts found.");
                }
                    
                return View(posts);
            }
            catch (Exception ex)
            {
                // Hata mesajını loglamak veya konsola yazdırmak
                Console.WriteLine(ex.Message);
                return View("Error", "An error occurred while fetching popular posts.");
            }

            
        }
    }
}