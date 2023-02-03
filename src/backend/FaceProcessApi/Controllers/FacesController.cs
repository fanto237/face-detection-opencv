using Microsoft.AspNetCore.Mvc;
using OpenCvSharp;

namespace FaceProcessApi.Controllers;

[ApiController]
[Route("/api/[Controller]")]
public class FacesController : Controller
{
    private readonly CascadeClassifier _cascadeClassifierFaces;

    public FacesController()
    {
        const string path = @"./Cascades/haarcascade_frontalface_alt.xml";
        _cascadeClassifierFaces = new CascadeClassifier();
        _cascadeClassifierFaces.Load(path);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Tuple<List<byte[]>, Guid>>> ExtractFaces(Guid orderId)
    {
        using var ms = new MemoryStream();
        await Request.Body.CopyToAsync(ms);
        var bytes = ms.ToArray();

        if (bytes.Length is 0)
            return BadRequest();

        return Ok(new Tuple<List<byte[]>, Guid>(DetectAndSave(bytes), orderId));
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