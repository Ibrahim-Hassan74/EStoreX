namespace EStoreX.Core.DTO
{
    public class EmailDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string HtmlMessage { get; set; } = string.Empty;
        public EmailDTO() { }
        public EmailDTO(string email, string subject, string htmlMessage)
        {
            Email = email;
            Subject = subject;
            HtmlMessage = htmlMessage;
        }
    }
}
