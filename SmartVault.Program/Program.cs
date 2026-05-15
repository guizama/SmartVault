using Microsoft.Extensions.Configuration;
using SmartVault.Application.UseCases.ExportSmithPropertyDocuments;
using SmartVault.Application.UseCases.ExportSmithPropertyDocuments.Handlers;
using SmartVault.Application.UseCases.GetAllFileSizes;
using SmartVault.Application.UseCases.GetAllFileSizes.Handlers;
using SmartVault.Infrastructure.Repositories;
using SmartVault.Shared;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SmartVault.Program
{
    partial class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var sharedDataPath = PathHelper.GetSharedFolder();

                var databasePath = Path.Combine(sharedDataPath, configuration["DatabaseFileName"]);

                var repository = new DocumentRepository(configuration, databasePath);

                var getSizesUseCase = new GetAllFileSizesUseCase(
                    new GetFileSizesHandler(repository));

                var totalSize = await getSizesUseCase.Execute();

                var exportUseCase = new ExportSmithPropertyDocumentsUseCase(
                    new GetDocumentsHandler(repository),
                    new FilterThirdDocumentsHandler(),
                    new ReadSmithPropertyHandler(),
                    new ExportDocumentsHandler());

                var requestPath = Path.Combine(sharedDataPath, "SmithProperty.txt");

                await exportUseCase.Execute(1, requestPath);

                Console.WriteLine($"Total file size: {totalSize} bytes");
                Console.WriteLine("Export completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}