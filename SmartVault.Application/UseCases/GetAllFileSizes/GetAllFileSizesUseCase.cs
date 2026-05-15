using SmartVault.Application.Common;
using SmartVault.Application.UseCases.GetAllFileSizes.Handlers;
using System.Threading.Tasks;

namespace SmartVault.Application.UseCases.GetAllFileSizes
{
    public class GetAllFileSizesUseCase
    {
        private readonly Handler<GetAllFileSizesRequest> pipeline;

        public GetAllFileSizesUseCase(GetFileSizesHandler handler)
        {
            pipeline = handler;
        }

        public async Task<long> Execute()
        {
            var request = new GetAllFileSizesRequest();

            await pipeline.Process(request);

            return request.TotalSize;
        }
    }
}