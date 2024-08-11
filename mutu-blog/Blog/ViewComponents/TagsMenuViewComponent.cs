using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.ViewComponents
{
    public class TagsMenuViewComponent:ViewComponent
    {
        private ITagRepository _tagRepository;
        public TagsMenuViewComponent(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _tagRepository.Tags.ToListAsync());
        }
    }
}