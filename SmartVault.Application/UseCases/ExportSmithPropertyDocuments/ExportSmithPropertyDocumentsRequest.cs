using SmartVault.Application.DTOs;
using System.Collections.Generic;

namespace SmartVault.Application.UseCases.ExportSmithPropertyDocuments
{
    public class ExportSmithPropertyDocumentsRequest
    {
        public int AccountId { get; set; }

        public List<DocumentDto> Documents { get; set; } = new();

        public List<string> Contents { get; set; } = new();

        public string OutputFilePath { get; set; } = "SmithProperty.txt";
    }
}