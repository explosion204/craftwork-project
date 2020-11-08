using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CraftworkProject.Services.Interfaces
{
    public interface IImageService
    {
        Task SaveImage(IFormFile file, string filePath);
    }
}