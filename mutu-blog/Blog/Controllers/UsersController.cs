using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Abstract;
using Blog.Entity;
using Blog.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Blog.Data.Concrete.EfCore;


namespace Blog.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private UserManager<User> _userManager;
        private RoleManager<Role> _roleManager;
        private SignInManager<User> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly BlogContext _context;


        public UsersController(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            IUserRepository userRepository,
            BlogContext context )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _context = context;
        }

        [HttpGet("User/Login")]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost("User/Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                
                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe,true);               
                    if(result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        await _userManager.SetLockoutEndDateAsync(user,null);

                        return RedirectToAction("Index","Home");
                    }
                    else if(result.IsLockedOut)
                    {
                        var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                        var timeLeft = lockoutDate.Value - DateTimeOffset.UtcNow;

                        ModelState.AddModelError("",$"Hesabınız kitlendi, Lütfen {timeLeft.Minutes} dakika sonra deneyiniz");
                    }
                    else
                    {
                        ModelState.AddModelError("","hatalı parola");
                    }
                }
                else
                {
                    ModelState.AddModelError("","hatalı email");
                }
            }
            return View();
        }

        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }


        [HttpGet("User/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("User/Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.Users.FirstOrDefault(x => x.UserName == model.UserName || x.Email == model.Email);
                
                if (user == null)
                {
                    user = new User
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        ProfilePhoto = "~/images/blank-profile-pic.jpg",
                        RoleId = "85512be7-2d8e-46aa-ae95-34a333e836de"
                    };
                    
                    // Kullanıcıyı veritabanına ekleyin
                    var result = await _userManager.CreateAsync(user, model.Password);
                    
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"Error: {error.Description}");
                        }
                    }
                }
                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpGet("User/Profile/{username}")]
        public IActionResult Profile(string username)
        {   
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                // Kullanıcı bulunamazsa 404 dönebilirsiniz
                return NotFound("Kullanıcı bulunamadı");
            }

            var writtenBlogs = _context.Posts
                    .Where(b => b.UserId == user.Id) // UserId ile blogları sorgula
                    .ToList();

            var comments = _context.Comments
                .Where(b => b.UserId == user.Id) // UserId ile blogları sorgula
                .Include(b => b.Post)
                .ToList();

            var viewModel = new UserProfileViewModel
            {
                Blogger = user,
                Blogs = writtenBlogs,
                Comments = comments
            };

            return View(viewModel);
        }

        [HttpGet("User/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}