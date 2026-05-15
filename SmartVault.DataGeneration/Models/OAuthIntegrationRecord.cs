using System;

namespace SmartVault.DataGeneration.Models
{
    public class OAuthIntegrationRecord
    {
        public int Id { get; set; }

        public string Provider { get; set; } = string.Empty;

        public string ClientId { get; set; } = string.Empty;

        public string ClientSecret { get; set; } = string.Empty;

        public string RedirectUrl { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }
    }
}