using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.ViewComponents
{
    public class NewestPostsViewComponent:ViewComponent
    {
        private IPostRepository _postRepository;

        public NewestPostsViewComponent(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _postRepository
                                .Posts
                                .OrderByDescending(p => p.CreatedDate)
                                .Take(3)
                                .ToListAsync());
        }
    }
}