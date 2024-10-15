using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name =  "Email")]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(25, ErrorMessage = "{0} alanı en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 4)]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Kullanıcı Adı Türkçe karakter ve boşluk içeremez.")]
        [Display(Name =  "Kullanıcı Adı")]
        public string UserName { get; set; } = null!;

        [Required]
        [StringLength(25, ErrorMessage = "{0} alanı en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{6,}$", ErrorMessage = "Şifre en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir.")]

        public string Password {get; set;} = null!;


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifreyi Onayla")]
        [Compare("Password", ErrorMessage = "Şifre ve onay şifresi eşleşmiyor.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}