using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entity;

namespace Blog.Models
{
    public class PostCreateViewModel
    {
        [Required]
        [Display(Name = "Başlık")]
        public string Title { get; set; }= null!;

        [Required]
        [Display(Name = "İçerik")]        
        public string Content { get; set; } = null!;

        [Required]
        [Display(Name = "Görsel")]
        public IFormFile PostImage { get; set; } = null!;

        [Required]
        [Display(Name = "Birincil Etiket")]
        public int PrimaryTagId { get; set; }
        
        // Tüm mevcut etiketleri tutar (GET request için)
        public List<Tag>? Tags { get; set; }

        // Seçilen etiket ID'lerini tutar (POST request için)
        [Required]
        [Display(Name = "Etiketler")]
        public string SelectedTagIdsString { get; set; } = null!;
    }
}