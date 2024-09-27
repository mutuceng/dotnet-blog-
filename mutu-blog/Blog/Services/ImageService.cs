using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveImage(IFormFile imageFile, string saveDirectory, string seoString)
        {
            // Sadece belirli dosya uzantılarına izin verelim
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new InvalidOperationException("Invalid file type. Only JPG, and PNG files are allowed.");
            }

            // Dosya adını kısa tutalım ve boşlukları "-" ile değiştirelim
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
                .Take(10).ToArray()).Replace(' ', '-');

            seoString = seoString.Replace(' ', '-');

            // SEO string uzunluğunu sınırla
            if (seoString.Length > 100)
            {
                seoString = seoString.Substring(0, 100); 
            }

            // Eşsiz bir dosya adı oluşturmak için tarih ve benzersiz bir isim ekleyelim
            imageName = $"{seoString}_{imageName}{fileExtension}";

            // wwwroot dizinine fiziksel yolu ekleyelim
            string directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, saveDirectory.TrimStart('~', '/'));

            // Klasörün var olup olmadığını kontrol et, yoksa oluştur
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine($"Directory created: {directoryPath}");
            }

            // Dosyanın kaydedileceği yolu manuel olarak belirtelim
            var imagePath = Path.Combine(directoryPath, imageName);
            Console.WriteLine("Saving file to: " + imagePath); // Dosya yolunu loglama


            // Dosyayı belirtilen yola kaydedelim
            try
            {
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message); // Hata loglama
                throw; // Hatanın dışarı fırlatılması
            }

            return imagePath; // Dosya adını geri döndürürüz
        }
    }
}