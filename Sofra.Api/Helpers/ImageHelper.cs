using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofra.Application.Helper
{

    public static  class ImageHelper
    {
        public static string UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded!");
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var uploadsPath = Path.Combine("wwwroot/Uploads");

            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }
            var filePath = Path.Combine(uploadsPath, fileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading image: {ex.Message}");
            }
        }
        private static bool ImageExtension(IFormFile fileName, string[] allowedExtensions)
        {
            var extension = Path.GetExtension(fileName.FileName).ToLower();
            return allowedExtensions.Contains(extension);
        }
        private static bool ImageLength(IFormFile fileName, long imageLength)
        {
            return fileName.Length <= imageLength;
        }
        public static bool DeleteImage(string file)
        {
            var imagename = Path.GetFileName(file);
            var uploadsPath = Path.Combine("wwwroot/Uploads/", imagename);
            if (System.IO.File.Exists(uploadsPath))
            {
                System.IO.File.Delete(uploadsPath);
                return true;
            }
            return false;
        }
    }
}
