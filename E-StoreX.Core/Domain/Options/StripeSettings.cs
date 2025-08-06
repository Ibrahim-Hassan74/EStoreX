namespace EStoreX.Core.Domain.Options
{
    public class StripeSettings
    {
        public string SigningSecret { get; set; } = string.Empty;
        public string PublishableKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
    }
}
