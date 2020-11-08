using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using CraftworkProject.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CraftworkProject.Services.Implementations
{
    public class ImageService : IImageService
    {
        public async Task SaveImage(IFormFile file, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // image postprocessing maybe in future...
        }
    }
}