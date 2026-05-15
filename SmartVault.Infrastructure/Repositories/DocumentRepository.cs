using Dapper;
using Microsoft.Extensions.Configuration;
using SmartVault.Application.DTOs;
using SmartVault.Application.Interfaces;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;

namespace SmartVault.Infrastructure.Repositories
{
    public class DocumentRepository(IConfiguration configuration, string databasePath) : IDocumentRepository
    {
        private readonly IConfiguration configuration = configuration;
        private readonly string databasePath = databasePath;

        public async Task<List<string>> GetAllDocumentPaths()
        {
            using var connection = new SQLiteConnection(string.Format(configuration["ConnectionStrings:DefaultConnection"] ?? "", Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "SharedFiles", configuration["DatabaseFileName"])));

            var result = await connection.QueryAsync<string>(
                "SELECT FilePath FROM Document");

            return [.. result];
        }

        public async Task<List<DocumentDto>> GetDocumentsByAccountId(int accountId)
        {
            using var connection = new SQLiteConnection(string.Format(configuration["ConnectionStrings:DefaultConnection"] ?? "", Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "SharedFiles", configuration["DatabaseFileName"])));

            var result = await connection.QueryAsync<DocumentDto>(@"SELECT Id, Name, FilePath 
                                                                    FROM Document 
                                                                    WHERE AccountId = @AccountId ORDER BY Id", new { AccountId = accountId });

            return [.. result];
        }
    }
}