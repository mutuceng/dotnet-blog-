using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Abstract;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.ViewComponents
{
    public class BestBloggersViewComponent:ViewComponent
    {
        private IPostRepository _postRepository;
        private IUserRepository _userRepository;

        public BestBloggersViewComponent(IPostRepository postRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var bestBloggers = await _postRepository
                                .Posts
                                .GroupBy(p => p.UserId)
                                .Select(async g => new BestBloggerViewModel
                                {
                                    UserId = g.Key,
                                    PostCount = g.Count(),
                                    FullName = (await _userRepository.GetUserByIdAsync(g.Key)).FullName,
                                    LinkedInProfile = (await _userRepository.GetUserByIdAsync(g.Key)).LinkedInProfile
                                })
                                .OrderByDescending(x => x.PostCount)
                                .Take(5)
                                .ToListAsync()

            return View(bestBloggers);        
        }
    }
}