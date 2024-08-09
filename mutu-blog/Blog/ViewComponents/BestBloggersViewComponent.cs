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
            var postGroups = await _postRepository
                                .Posts
                                .GroupBy(p => p.UserId)
                                .ToListAsync();

            var bestBloggers = postGroups
                                .Join(_userRepository.Users,
                                    groupedPosts => groupedPosts.Key,
                                    user => user.Id,
                                    (groupedPosts, user) => new BestBloggerViewModel
                                    {
                                        UserId = user.Id,
                                        FullName = user.FullName,
                                        UserName = user.UserName,
                                        ProfilePhoto = user.ProfilePhoto,
                                        Email = user.Email,
                                        LinkedInProfile = user.LinkedInProfile,
                                        PostCount = groupedPosts.Count()
                                    })
                                .OrderByDescending(x => x.PostCount)
                                .Take(5)
                                .ToList();

            return View(bestBloggers);        
        }
    }
}