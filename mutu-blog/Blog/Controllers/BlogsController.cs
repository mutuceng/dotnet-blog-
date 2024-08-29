using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Concrete.EfCore;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blog.Controllers
{
    [Route("[controller]")]
    public class BlogsController : Controller
    {
        private readonly BlogContext _context;
        

        public BlogsController(BlogContext context)
        {
            _context = context;
        }

        [Route("")]
        public IActionResult Index()
        {
            var posts = _context.Posts;
            return View(posts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("/Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("Blog/Create")]
        public IActionResult Create()
        {
            var model = new PostCreateViewModel
            {
                Tags = GetAvailableTags()  // Etiketleri alıyoruz
            };
            return View(model);
        }

        [HttpPost("Blog/Create")]
        public IActionResult Create(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Model verilerini kaydetme işlemleri burada yapılır

                return RedirectToAction("Index");
            }

            model.Tags = GetAvailableTags();  // Hatalı gönderimde etiket listesini yeniden yükleyin
            return View(model);
        }

        private List<string> GetAvailableTags()
        {
            return _context.Tags.Select(tag => tag.Name).ToList();
        }

        [HttpGet("Blog/Details")]
        public IActionResult Details(int id)
        {   
            var blogPost = _context.Posts.FirstOrDefault(b => b.PostId == id);
            if (blogPost == null)
            {
                return View();
            }

            var viewModel = new BlogDetailsViewModel
            {
                Blog = blogPost,
                Comments = _context.Comments.Where(c => c.PostId == id).ToList()
            };

            return View(viewModel);
        }
    }
}