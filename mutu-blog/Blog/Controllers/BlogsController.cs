using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Data.Abstract;
using Blog.Data.Concrete.EfCore;
using Blog.Entity;
using Blog.Models;
using Blog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Blog.Controllers
{
    [Route("[controller]")]
    public class BlogsController : Controller
    {
        private readonly BlogContext _context;
        private readonly ImageService _imageService;
        private IPostRepository _postRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        

        public BlogsController(BlogContext context, IPostRepository postRepository, IWebHostEnvironment WebHostEnvironment,ImageService imageService)
        {
            _context = context;
            _postRepository = postRepository;
            _webHostEnvironment = WebHostEnvironment;
            _imageService = imageService;
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
        public async Task<IActionResult> CreateAsync(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }

                    List<int> selectedTags = new List<int>();
                    List<Tag> postTags = new List<Tag>();

                    if (!string.IsNullOrEmpty(model.SelectedTagIdsString))
                    {
                        selectedTags = model.SelectedTagIdsString.Split(',')
                                            .Select(int.Parse).ToList();
                    }

                    if (selectedTags != null && selectedTags.Any())
                    {
                        postTags = _context.Tags.Where(t => selectedTags.Contains(t.TagId)).ToList();
                    }

if (model.PostImage != null)
{
    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
    var fileExtension = Path.GetExtension(model.PostImage.FileName).ToLower();

    if (!allowedExtensions.Contains(fileExtension))
    {
        ModelState.AddModelError("PostImage", "Invalid file type. Only JPG, and PNG files are allowed.");
        model.Tags = GetAvailableTags();
        return View(model);
    }

    var fileName = $"{Guid.NewGuid()}-{Path.GetFileName(model.PostImage.FileName)}";
    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
    
    try
    {
        using (var fileStream = new FileStream(imagePath, FileMode.Create))
        {
            await model.PostImage.CopyToAsync(fileStream);
        }
    }
    catch (Exception ex)
    {
        ModelState.AddModelError("", "There was a problem saving the image. Please try again.");
        model.Tags = GetAvailableTags();
        return View(model);
    }

    // Sanal kök dizini belirterek kaydetme
    var dbImagePath = Path.Combine("~/images", fileName).Replace("\\", "/");  // Windows ortamında ters slash yerine normal slash kullanmak için

    var random = new Random();
    var postid = random.Next(100000, 1000000);

    var postEntity = new Post
    {
        PostId = postid,
        Title = model.Title,
        Content = model.Content,
        UserId = userId,
        CreatedDate = DateTime.Now,
        PostImage = dbImagePath,  // Her zaman sanal kökle başlayan formatta kaydediliyor
        PrimaryTagId = model.PrimaryTagId,
        IsActive = false,
        Tags = postTags
    };

    try
    {
        _postRepository.CreatePost(postEntity);
        await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        ModelState.AddModelError("", "There was a problem saving the post. Please try again.");
        model.Tags = GetAvailableTags();
        return View(model);
    }
}
else
{
    ModelState.AddModelError("PostImage", "Image file is required.");
    model.Tags = GetAvailableTags();
    return View(model);
}

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                    model.Tags = GetAvailableTags();
                    return View(model);
                }

                return RedirectToAction("Index");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage)
                                            .ToList();
                
                ViewBag.ErrorMessages = errors;  // Hataları view'e gönderiyoruz
                
                model.Tags = GetAvailableTags();
                return View(model);
            }
        }

        private List<Tag> GetAvailableTags()
        {
            return _context.Tags.ToList();
        }

        [HttpGet("Blog/Details")]
        public IActionResult Details(int id)
        {   
            var blogPost = _context.Posts.Include(b => b.Tags).FirstOrDefault(b => b.PostId == id);
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
                Tags = blogPost.Tags.ToList()
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