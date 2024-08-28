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


namespace Blog.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private UserManager<User> _userManager;
        private RoleManager<Role> _roleManager;
        private SignInManager<User> _signInManager;
        private readonly IUserRepository _userRepository;


        public UsersController(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            IUserRepository userRepository )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
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
            return RedirectToAction("User/Login");
        }


        [HttpGet("User/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("User/Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = _userRepository.Users.FirstOrDefault(x => x.UserName == model.UserName || x.Email == model.Email);
                if(user == null)
                {
                    _userRepository.CreateUser(
                        user = new User {
                            UserName  = model.UserName,
                            Email = model.Email,
                            ProfilePhoto = "~/images/blank-profile-pic.jpg.jpg",
                            RoleId = "85512be7-2d8e-46aa-ae95-34a333e836de"
                    });

                    var result = await _userManager.CreateAsync(user, model.Password);
                }
                return RedirectToAction("User/Login");
            }
            return View(model);
        }

        [HttpGet("User/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}