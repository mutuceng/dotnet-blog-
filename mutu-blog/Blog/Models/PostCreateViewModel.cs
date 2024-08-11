using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class PostCreateViewModel
    {
        [Required]
        [Display(Name = "Başlık")]
        public string? Title { get; set; }

        [Required]
        [Display(Name = "İçerik")]        
        public string? Content { get; set; }

        [Required]
        [Display(Name = "Görsel")]
        public IFormFile? PostImage { get; set; }

        [Required]
        [Display(Name = "Birincil Etiket")]
        public string? PrimaryTagName { get; set; }
        
        [Required]
        [Display(Name = "Etiketler")]
        public List<string>? Tags { get; set; } 
    }
}