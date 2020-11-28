using System.Threading.Tasks;

namespace CraftworkProject.Services.Interfaces
{
    public interface IEmailService
    {
        // TODO: send email on order state change
        Task SendEmailAsync(string receiver, string subject, string body);
    }
}