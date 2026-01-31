namespace Daira.Infrastructure.Settings
{
    public class EmailSettings
    {
        public const string SectionName = "EmailSettings";
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; } = 587;
        public string SmtpUser { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public bool UseSsl { get; set; } = true;
    }
}
