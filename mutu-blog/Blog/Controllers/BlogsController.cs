using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Abstract;
using Blog.Data.Concrete.EfCore;
using Blog.Entity;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Blog.Controllers
{
    [Route("[controller]")]
    public class BlogsController : Controller
    {
        private readonly BlogContext _context;
        private IPostRepository _postRepository;
        private ITagRepository _tagRepository;
        

        public BlogsController(BlogContext context, IPostRepository postRepository, ITagRepository tagRepository)
        {
            _context = context;
            _postRepository = postRepository;
            _tagRepository = tagRepository;
        }

        [Route("")]
        public IActionResult Index()
        {
            // Tüm postları almak için IQueryable tanımlıyoruz
            IQueryable<Post> posts = _context.Posts;

            return View(posts);
        }

        [HttpGet("Blog/Tag/{tagId}")]
        public IActionResult Index(int? tagId)
        {
            // Tüm postları almak için IQueryable tanımlıyoruz
            IQueryable<Post> posts = _context.Posts;

            // Eğer tagId geçerliyse, postları filtreliyoruz
            if (tagId.HasValue)
            {
                    
                var filteredPosts = 
                    from post in _context.Posts
                    join postTag in _context.PostTags on post.PostId equals (int)postTag["PostId"]
                    where (int)postTag["TagId"] == tagId // Kullanıcının gönderdiği TagID
                    select post;;

                 return View(filteredPosts);
            }

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

            var blogger = _context.Users.FirstOrDefault(u => u.Id == blogPost.UserId);
            
            var viewModel = new BlogDetailsViewModel
            {
                Blog = blogPost,
                Comments = _context.Comments.Where(c => c.PostId == id).Include(c => c.User).ToList(),
                Blogger = blogger ?? new User { UserName = "" },
            };

            return View(viewModel);
        }
        
        // public IActionResult PostsByTag(int tagId)
        // {


        //     var filteredPosts = 
        //         from post in _context.Posts
        //         join postTag in _context.PostTags on post.PostId equals (int)postTag["PostId"]
        //          where (int)postTag["TagId"] == tagId // Kullanıcının gönderdiği TagID
        //         select post;



        //     return View(filteredPosts);
        // }
    }
}