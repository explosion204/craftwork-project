namespace CraftworkProject.Web.Service
{
    public class AppOptions
    {
        public const string SectionName = "Project";
        
        public string ConnectionString { get; set; }
        public string CompanyName { get; set; }
        public string CompanyShortDesc { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
    }
}