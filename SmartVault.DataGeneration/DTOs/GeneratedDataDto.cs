using SmartVault.DataGeneration.Models;
using System.Collections.Generic;

namespace SmartVault.DataGeneration.DTOs
{
    public class GeneratedDataDto
    {
        public List<AccountRecord> Accounts { get; set; } = new();

        public List<UserRecord> Users { get; set; } = new();

        public List<DocumentRecord> Documents { get; set; } = new();
        public List<OAuthIntegrationRecord> OAuthIntegrations { get; set; } = new();
    }
}