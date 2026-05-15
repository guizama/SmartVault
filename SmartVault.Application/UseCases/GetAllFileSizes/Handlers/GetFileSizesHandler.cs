using SmartVault.Application.Common;
using SmartVault.Application.Interfaces;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartVault.Application.UseCases.GetAllFileSizes.Handlers
{
    public class GetFileSizesHandler : Handler<GetAllFileSizesRequest>
    {
        private readonly IDocumentRepository repository;

        public GetFileSizesHandler(IDocumentRepository repository)
        {
            this.repository = repository;
        }

        public override async Task Process(GetAllFileSizesRequest request)
        {
            var paths = await repository.GetAllDocumentPaths();

            long total = 0;

            foreach (var path in paths.Distinct())
            {
                if (!File.Exists(path))
                    continue;

                total += new FileInfo(path).Length;
            }

            request.TotalSize = total;

            if (Successor != null)
                await Successor.Process(request);
        }
    }
}