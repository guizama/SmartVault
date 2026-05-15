using SmartVault.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartVault.Application.Interfaces
{
    public interface IDocumentRepository
    {
        Task<List<string>> GetAllDocumentPaths();

        Task<List<DocumentDto>> GetDocumentsByAccountId(int accountId);
    }

}
