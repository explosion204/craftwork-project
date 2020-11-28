namespace CraftworkProject.Web.Service
{
    public class LoggingOptions
    {
        public const string SectionName = "Logging";
        
        public string CommonLogFilePath { get; set; }
        public string ErrorLogFilePath { get; set; }
    }
}