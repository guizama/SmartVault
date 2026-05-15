using SmartVault.Application.Common;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SmartVault.Application.UseCases.ExportSmithPropertyDocuments.Handlers
{
    public class ExportDocumentsHandler : Handler<ExportSmithPropertyDocumentsRequest>
    {
        public override async Task Process(ExportSmithPropertyDocumentsRequest request)
        {
            var output = string.Join(Environment.NewLine, request.Contents);

            await File.WriteAllTextAsync(request.OutputFilePath, output);

            if (Successor != null)
                await Successor.Process(request);
        }
    }
}