namespace EStoreX.Core.DTO.Account.Requests
{
    public class EmailDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string HtmlMessage { get; set; } = string.Empty;
        public List<EmailAttachmentDTO>? Attachments { get; set; }
        public EmailDTO() { }
        public EmailDTO(string email, string subject, string htmlMessage)
        {
            Email = email;
            Subject = subject;
            HtmlMessage = htmlMessage;
        }
    }
    public class EmailAttachmentDTO
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public byte[] FileBytes { get; set; } = Array.Empty<byte>();
    }
}
