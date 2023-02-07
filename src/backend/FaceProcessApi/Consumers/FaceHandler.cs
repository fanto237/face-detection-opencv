using OpenCvSharp;

namespace FaceProcessApi.Consumers;

public class  FaceHandler : IFaceHandler
{
    private readonly CascadeClassifier _cascadeClassifierFaces;

    public FaceHandler()
    {
        const string path = @"./Cascades/haarcascade_frontalface_alt.xml";
        _cascadeClassifierFaces = new CascadeClassifier();
        _cascadeClassifierFaces.Load(path);
    }

    public List<byte[]> ExtractFaces(byte[] imageData)
    {
        var bytesList = DetectAndSave(imageData);
        return bytesList;
    }

    private List<byte[]> DetectAndSave(byte[] bytes)
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
            face.SaveImage($"face_number-{i}.jpg", new ImageEncodingParam(ImwriteFlags.JpegProgressive, 255));
        }

        return facesBytes;
    }
}