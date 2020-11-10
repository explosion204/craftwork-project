using System.Threading.Tasks;

namespace CraftworkProject.Services.Interfaces
{
    public interface ISmsService
    {
        Task<bool> SendAsync(string receiver, string message);
    }
}