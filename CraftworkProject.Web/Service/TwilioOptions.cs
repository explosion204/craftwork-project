namespace CraftworkProject.Web.Service
{
    public class TwilioOptions
    {
        public const string SectionName = "Twilio";
        
        public string Sender { get; set; }
        public string AccountSid { get; set; }
        public string AuthToken { get; set; }
    }
}