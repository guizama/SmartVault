using FluentAssertions;
using Moq;
using SmartVault.Application.DTOs;
using SmartVault.Application.Interfaces;
using SmartVault.Application.UseCases.ExportSmithPropertyDocuments;
using SmartVault.Application.UseCases.ExportSmithPropertyDocuments.Handlers;

namespace SmartVault.Tests.UseCases.ExportSmithPropertyDocuments;

public class ExportSmithPropertyDocumentsUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldExportOnlyEveryThirdDocumentThatContainsSmithProperty()
    {
        var tempDir = Directory.CreateTempSubdirectory("sv-tests-");

        try
        {
            var doc1 = Path.Combine(tempDir.FullName, "1.txt");
            var doc2 = Path.Combine(tempDir.FullName, "2.txt");
            var doc3 = Path.Combine(tempDir.FullName, "3.txt");
            var doc4 = Path.Combine(tempDir.FullName, "4.txt");
            var doc5 = Path.Combine(tempDir.FullName, "5.txt");
            var doc6 = Path.Combine(tempDir.FullName, "6.txt");

            await File.WriteAllTextAsync(doc1, "Smith Property - a");
            await File.WriteAllTextAsync(doc2, "Smith Property - b");
            await File.WriteAllTextAsync(doc3, "Smith Property - c");
            await File.WriteAllTextAsync(doc4, "Smith Property - d");
            await File.WriteAllTextAsync(doc5, "Smith Property - e");
            await File.WriteAllTextAsync(doc6, "no match");

            var repo = new Mock<IDocumentRepository>(MockBehavior.Strict);
            repo.Setup(r => r.GetDocumentsByAccountId(1))
                .ReturnsAsync(new List<DocumentDto>
                {
                    new() { FilePath = doc1 },
                    new() { FilePath = doc2 },
                    new() { FilePath = doc3 },
                    new() { FilePath = doc4 },
                    new() { FilePath = doc5 },
                    new() { FilePath = doc6 },
                });

            var useCase = new ExportSmithPropertyDocumentsUseCase(
                new GetDocumentsHandler(repo.Object),
                new FilterThirdDocumentsHandler(),
                new ReadSmithPropertyHandler(),
                new ExportDocumentsHandler());

            var outputPath = Path.Combine(tempDir.FullName, "out.txt");
            await useCase.Execute(1, outputPath);

            var output = await File.ReadAllTextAsync(outputPath);

            output.Should().Be("Smith Property - c");
            repo.VerifyAll();
        }
        finally
        {
            tempDir.Delete(recursive: true);
        }
    }
}
