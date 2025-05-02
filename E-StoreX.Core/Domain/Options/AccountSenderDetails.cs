using System;

namespace EStoreX.Core.Domain.Options
{
    public class AccountSenderDetails
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Host { get; set; }
        public int Port { get; set; }
    }
}
