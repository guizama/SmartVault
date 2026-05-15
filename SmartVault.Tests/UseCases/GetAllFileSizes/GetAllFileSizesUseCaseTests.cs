using FluentAssertions;
using Moq;
using SmartVault.Application.Interfaces;
using SmartVault.Application.UseCases.GetAllFileSizes;
using SmartVault.Application.UseCases.GetAllFileSizes.Handlers;

namespace SmartVault.Tests.UseCases.GetAllFileSizes;

public class GetAllFileSizesUseCaseTests
{
    [Fact]
    public async Task Execute_WhenRepositoryReturnsPaths_ShouldReturnSumOfExistingDistinctFileSizes()
    {
        var tempDir = Directory.CreateTempSubdirectory("sv-tests-");

        try
        {
            var file1 = Path.Combine(tempDir.FullName, "a.txt");
            var file2 = Path.Combine(tempDir.FullName, "b.txt");
            var missing = Path.Combine(tempDir.FullName, "missing.txt");

            await File.WriteAllTextAsync(file1, "12345"); // 5 bytes
            await File.WriteAllTextAsync(file2, "1234567890"); // 10 bytes

            var repo = new Mock<IDocumentRepository>(MockBehavior.Strict);
            repo.Setup(r => r.GetAllDocumentPaths())
                .ReturnsAsync(new List<string> { file1, file1, file2, missing });

            var useCase = new GetAllFileSizesUseCase(new GetFileSizesHandler(repo.Object));

            var total = await useCase.Execute();

            total.Should().Be(15);
            repo.VerifyAll();
        }
        finally
        {
            tempDir.Delete(recursive: true);
        }
    }
}
