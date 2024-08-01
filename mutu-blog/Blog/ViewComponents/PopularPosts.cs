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
            return View(await _postRepository
                                .Posts
                                .Select(post => new 
                                {
                                    Post = post,
                                    PopularityScore = post.PostInteractions.Sum(pi => pi.ViewCount + pi.TimeSpent.TotalSeconds/1000)
                                })
                                .OrderByDescending(x => x.PopularityScore)
                                .Take(6)
                                .Select(x => x.Post)
                                .ToListAsync()
            );
        }
    }
}