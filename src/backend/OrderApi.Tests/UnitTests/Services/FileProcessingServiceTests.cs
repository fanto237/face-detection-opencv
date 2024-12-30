using System.Text;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using OrderApi.Services;

namespace OrderApi.Tests.Services;

public class FileProcessingServiceTests
{
    private readonly IFileProcessingService _fileProcessingService = new FileProcessingService();

    [Fact]
    public async Task ConvertToBytes_ShouldReturnByteArray()
    {
        // Arrange
        var fileContent = "This is a test file";
        var fileName = "test.txt";
        var fileMock = new FormFile(
            new MemoryStream(Encoding.UTF8.GetBytes(fileContent)),
            0,
            fileContent.Length,
            "Data",
            fileName
        )
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain"
        };

        // Act
        var result = await _fileProcessingService.ConvertToBytes(fileMock);

        // Assert
        var expectedBytes = Encoding.UTF8.GetBytes(fileContent);
        Assert.Equal(expectedBytes, result);
    }

    [Fact]
    public void GenerateImageName_ShouldReturnCorrectImageName()
    {
        // Arrange
        var fileMock = Substitute.For<IFormFile>();
        fileMock.FileName.Returns("example_image.png");

        // Act
        var result = _fileProcessingService.GenerateImageName(fileMock);

        // Assert
        var expectedExtension = Path.GetExtension(fileMock.FileName);
        Assert.Contains(expectedExtension, result);
        Assert.True(result.Length > expectedExtension.Length);
    }
}