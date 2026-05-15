using SmartVault.Application.Common;
using SmartVault.Application.Interfaces;
using System.Threading.Tasks;

namespace SmartVault.Application.UseCases.ExportSmithPropertyDocuments.Handlers
{
    public class GetDocumentsHandler : Handler<ExportSmithPropertyDocumentsRequest>
    {
        private readonly IDocumentRepository repository;

        public GetDocumentsHandler(IDocumentRepository repository)
        {
            this.repository = repository;
        }

        public override async Task Process(ExportSmithPropertyDocumentsRequest request)
        {
            request.Documents = await repository.GetDocumentsByAccountId(request.AccountId);

            if (Successor != null)
                await Successor.Process(request);
        }
    }
}