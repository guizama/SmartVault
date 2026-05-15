using FluentAssertions;
using SmartVault.Application.UseCases.ExportSmithPropertyDocuments;
using SmartVault.Application.UseCases.ExportSmithPropertyDocuments.Handlers;

namespace SmartVault.Tests.UseCases.ExportSmithPropertyDocuments;

public class ExportDocumentsHandlerTests
{
    [Fact]
    public async Task Process_ShouldWriteJoinedContentsToOutputFile()
    {
        var tempDir = Directory.CreateTempSubdirectory("sv-tests-");

        try
        {
            var output = Path.Combine(tempDir.FullName, "out.txt");

            var request = new ExportSmithPropertyDocumentsRequest
            {
                OutputFilePath = output,
                Contents = new List<string> { "line1", "line2" }
            };

            var handler = new ExportDocumentsHandler();

            await handler.Process(request);

            File.Exists(output).Should().BeTrue();
            var text = await File.ReadAllTextAsync(output);
            text.Should().Be($"line1{Environment.NewLine}line2");
        }
        finally
        {
            tempDir.Delete(recursive: true);
        }
    }
}
