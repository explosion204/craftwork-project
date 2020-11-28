using System.Threading.Tasks;
using CraftworkProject.Services.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace CraftworkProject.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly string _sender;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _username;
        private readonly string _password;
        public EmailService(string sender, string smtpServer, int smtpPort, string username, string password)
        {
            _sender = sender;
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _username = username;
            _password = password;
        }

        public async Task SendEmailAsync(string receiver, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_sender));
            emailMessage.To.Add(new MailboxAddress(receiver));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Text) {Text = body};

            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync(_smtpServer, _smtpPort);
                await smtpClient.AuthenticateAsync(_username, _password);
                await smtpClient.SendAsync(emailMessage);
                await smtpClient.DisconnectAsync(true);
            }
        }
    }
}