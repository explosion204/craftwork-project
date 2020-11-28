namespace CraftworkProject.Web.Service
{
    public class MailingOptions
    {
        public const string SectionName = "Mailing";
        
        public string Sender { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}