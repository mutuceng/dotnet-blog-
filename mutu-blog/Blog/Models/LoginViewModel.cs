using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name =  "Email")]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password {get; set;} = null!;

        public bool RememberMe {get; set;} = false;
    }
}