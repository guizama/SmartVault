using SmartVault.Application.Common;
using SmartVault.Application.UseCases.ExportSmithPropertyDocuments.Handlers;
using System.Threading.Tasks;

namespace SmartVault.Application.UseCases.ExportSmithPropertyDocuments
{
    public class ExportSmithPropertyDocumentsUseCase
    {
        private readonly Handler<ExportSmithPropertyDocumentsRequest> pipeline;

        public ExportSmithPropertyDocumentsUseCase(
            GetDocumentsHandler getDocumentsHandler,
            FilterThirdDocumentsHandler filterThirdDocumentsHandler,
            ReadSmithPropertyHandler readSmithPropertyHandler,
            ExportDocumentsHandler exportDocumentsHandler)
        {
            getDocumentsHandler
                .SetSuccessor(filterThirdDocumentsHandler)
                .SetSuccessor(readSmithPropertyHandler)
                .SetSuccessor(exportDocumentsHandler);

            pipeline = getDocumentsHandler;
        }

        public async Task Execute(int accountId, string outputPath)
        {
            var request = new ExportSmithPropertyDocumentsRequest
            {
                AccountId = accountId,
                OutputFilePath = outputPath
            };

            await pipeline.Process(request);
        }
    }
}