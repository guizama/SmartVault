using SmartVault.Application.Common;
using System.Linq;
using System.Threading.Tasks;

namespace SmartVault.Application.UseCases.ExportSmithPropertyDocuments.Handlers
{
    public class FilterThirdDocumentsHandler : Handler<ExportSmithPropertyDocumentsRequest>
    {
        public override async Task Process(ExportSmithPropertyDocumentsRequest request)
        {
            request.Documents = request.Documents
                .Where((x, index) => (index + 1) % 3 == 0)
                .ToList();

            if (Successor != null)
                await Successor.Process(request);
        }
    }
}