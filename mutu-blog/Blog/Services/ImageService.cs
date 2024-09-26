using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class ImageService
    {
        public async Task<string> SaveImage(IFormFile imageFile, string saveDirectory, string seoString)
        {
            // Sadece belirli dosya uzantılarına izin verelim (güvenlik açısından önemli)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png"};
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new InvalidOperationException("Invalid file type. Only JPG, and PNG files are allowed.");
            }

            // Dosya adını kısa tutalım ve boşlukları "-" ile değiştirelim
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
                .Take(10).ToArray()).Replace(' ', '-');

            seoString = seoString.Replace(' ', '-');

            // SEO string uzunluğunu sınırla (örneğin, 100 karakterle)
            if (seoString.Length > 100)
            {
                seoString = seoString.Substring(0, 100); // İlk 100 karakteri al
            }

            // Eşsiz bir dosya adı oluşturmak için tarih ve benzersiz bir isim ekleyelim
            imageName = $"{seoString}_{imageName}{fileExtension}";

            // Dosyanın kaydedileceği yolu manuel olarak belirtelim
            var imagePath = Path.Combine(saveDirectory, imageName);

            // Dosyayı belirtilen yola kaydedelim
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName; // Dosya adını geri döndürürüz
        }
    }
}