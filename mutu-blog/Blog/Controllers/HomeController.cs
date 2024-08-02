using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Blog.Data.Abstract;

namespace Blog.Controllers;

public class HomeController : Controller
{
    private IPostRepository _postRepository;
    private ITagRepository _tagRepository;
    private IUserRepository _userRepository;
    public HomeController(IPostRepository postRepository,IUserRepository userRepository, ITagRepository tagRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _tagRepository = tagRepository;
    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Hakkimda()
    {
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
