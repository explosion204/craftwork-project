using System.Threading.Tasks;

namespace CraftworkProject.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string receiver, string subject, string body);
    }
}