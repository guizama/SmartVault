using FluentAssertions;
using SmartVault.Application.DTOs;
using SmartVault.Application.UseCases.ExportSmithPropertyDocuments;
using SmartVault.Application.UseCases.ExportSmithPropertyDocuments.Handlers;

namespace SmartVault.Tests.UseCases.ExportSmithPropertyDocuments;

public class FilterThirdDocumentsHandlerTests
{
    [Fact]
    public async Task Process_ShouldKeepEveryThirdDocument_UsingOneBasedIndexing()
    {
        var request = new ExportSmithPropertyDocumentsRequest
        {
            Documents = new List<DocumentDto>
            {
                new() { Id = 1 },
                new() { Id = 2 },
                new() { Id = 3 },
                new() { Id = 4 },
                new() { Id = 5 },
                new() { Id = 6 },
                new() { Id = 7 },
            }
        };

        var handler = new FilterThirdDocumentsHandler();

        await handler.Process(request);

        request.Documents.Select(d => d.Id).Should().Equal(3, 6);
    }
}
