using FluentAssertions;
using SmartVault.Application.DTOs;
using SmartVault.Application.UseCases.ExportSmithPropertyDocuments;
using SmartVault.Application.UseCases.ExportSmithPropertyDocuments.Handlers;

namespace SmartVault.Tests.UseCases.ExportSmithPropertyDocuments;

public class ReadSmithPropertyHandlerTests
{
    [Fact]
    public async Task Process_ShouldReadOnlyExistingFiles_AndOnlyThoseContainingSmithProperty()
    {
        var tempDir = Directory.CreateTempSubdirectory("sv-tests-");

        try
        {
            var ok = Path.Combine(tempDir.FullName, "ok.txt");
            var notOk = Path.Combine(tempDir.FullName, "not-ok.txt");
            var missing = Path.Combine(tempDir.FullName, "missing.txt");

            await File.WriteAllTextAsync(ok, "Hello Smith Property world");
            await File.WriteAllTextAsync(notOk, "No match here");

            var request = new ExportSmithPropertyDocumentsRequest
            {
                Documents = new List<DocumentDto>
                {
                    new() { FilePath = ok },
                    new() { FilePath = notOk },
                    new() { FilePath = missing }
                }
            };

            var handler = new ReadSmithPropertyHandler();

            await handler.Process(request);

            request.Contents.Should().HaveCount(1);
            request.Contents[0].Should().Contain("Smith Property");
        }
        finally
        {
            tempDir.Delete(recursive: true);
        }
    }
}
