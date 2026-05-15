using SmartVault.Application.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartVault.Application.UseCases.ExportSmithPropertyDocuments.Handlers
{
    public class ReadSmithPropertyHandler : Handler<ExportSmithPropertyDocumentsRequest>
    {
        public override async Task Process(ExportSmithPropertyDocumentsRequest request)
        {
            var uniqueFiles = request.Documents
                .Where(d => File.Exists(d.FilePath))
                .Select(d => d.FilePath)
                .Distinct();

            var contents = await Task.WhenAll(
                uniqueFiles
                    .Select(async d =>
                    {
                        var content = await File.ReadAllTextAsync(d);
                        return content.Contains("Smith Property") ? content : null;
                    }));

            request.Contents.AddRange(
                contents.Where(c => c != null)!);

            if (Successor != null)
                await Successor.Process(request);
        }
    }
}