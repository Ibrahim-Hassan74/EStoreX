namespace EStoreX.Core.Domain.Options
{
    public class SecuritySettings
    {
        public string ApiKeyHeaderName { get; set; } = string.Empty;
        public List<string> AllowedStaticPaths { get; set; } = new();
        public List<string> AllowedStaticExtensions { get; set; } = new();
    }

}
