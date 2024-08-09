using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class BestBloggerViewModel
    {
    public string UserId { get; set; }  =  null!;       // Kullanıcının ID'si
    public string? FullName { get; set; }   // Kullanıcının tam adı
    public string UserName { get; set; }  =  null!;      // Kullanıcının tam adı
    public string? Email {get; set;}
    public string? LinkedInProfile { get; set; }  // Kullanıcının LinkedIn profil URL'si
    public string? ProfilePhoto { get; set; }  // Kullanıcının Twitter profil URL
    public int PostCount { get; set; }       // Kullanıcının yazdığı post sayısı
    }
}