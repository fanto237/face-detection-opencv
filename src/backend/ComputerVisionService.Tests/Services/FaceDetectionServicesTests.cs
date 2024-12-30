using ComputerVisionService.Services;

namespace ComputerVisionService.Tests.Services;

public class FaceDetectionServicesTests
{
    private readonly IFaceDetectionServices _faceDetectionServices = new FaceDetectionServices();

    [Fact]
    public async Task ExtractFaces_ShouldReturnFaceData()
    {
        // Arrange
        var testImage = await File.ReadAllBytesAsync("Services/best-image.jpg");

        // Act
        var faces = await _faceDetectionServices.ExtractFaces(testImage);

        // Assert
        Assert.NotEmpty(faces);
        Assert.All(faces, Assert.NotNull);
    }

    [Fact]
    public async Task ExtractFaces_NoFacesDetected_ShouldReturnEmptyList()
    {
        // Arrange
        var testImage = await File.ReadAllBytesAsync("Services/no-face.jpg");

        // Act
        var faces = await _faceDetectionServices.ExtractFaces(testImage);

        // Assert
        Assert.Empty(faces);
    }
}