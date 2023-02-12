using OpenCvSharp;

namespace ComputerVisionService.Consumers;

public class  FaceHandler : IFaceHandler
{
    private readonly CascadeClassifier _cascadeClassifierFaces;

    public FaceHandler()
    {
        const string path = @"./Cascades/haarcascade_frontalface_alt.xml";
        _cascadeClassifierFaces = new CascadeClassifier();
        _cascadeClassifierFaces.Load(path);
    }

    public async Task<List<byte[]>> ExtractFaces(byte[] imageData)
    {
        return await DetectAndSave(imageData);
        
    }

    private Task<List<byte[]>> DetectAndSave(byte[] bytes)
    {
        using var srcImage = Cv2.ImDecode(bytes, ImreadModes.Color);
        using var grayScale = new Mat();
        Cv2.CvtColor(srcImage, grayScale, ColorConversionCodes.BGR2GRAY);
        Cv2.EqualizeHist(grayScale, grayScale);

        var facesRectangles = _cascadeClassifierFaces.DetectMultiScale(grayScale, minNeighbors: 6,
            flags: HaarDetectionTypes.DoRoughSearch,
            minSize: new Size(60, 60));
        var facesBytes = new List<byte[]>();

        for (var i = 0; i < facesRectangles.Length; ++i)
        {
            using var face = new Mat(srcImage, facesRectangles[i]);
            facesBytes.Add(face.ToBytes(".jpg"));
            face.SaveImage($"./Images/face_number-{i}.jpg", new ImageEncodingParam(ImwriteFlags.JpegProgressive, 255));
        }

        return Task.FromResult(facesBytes);
    }
}