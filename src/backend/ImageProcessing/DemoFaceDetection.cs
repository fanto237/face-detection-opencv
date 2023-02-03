using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http.Headers;
using System.Text.Json;
using OpenCvSharp;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace ImageProcessing;

// ReSharper disable InvalidXmlDocComment
public class DemoFaceDetection
{
    private readonly CascadeClassifier _cascadeClassifierEyes;
    private readonly CascadeClassifier _cascadeClassifierFaces;
    private readonly HttpClient _client = new();


    public DemoFaceDetection()
    {
        _cascadeClassifierFaces = new CascadeClassifier();
        _cascadeClassifierFaces.Load(@"Data/haarcascade_frontalface_alt.xml");
        _cascadeClassifierEyes = new CascadeClassifier();
        _cascadeClassifierEyes.Load(@"Data/haarcascade_eye.xml");
    }

    public void DetectFace(Mat srcImage)
    {
// loading the haarcascade file we want to use
        var color = Scalar.Red;

// loading the image "face.jpg" and storing it in a variable
        // using var srcImage = new Mat(@"./Images/face.jpg");

        using var grayImage = new Mat();

        /**
         * we are calling the cv2 convert color and passing as argument
         * the source image what we want to convert ( srcImage )
         * where we want that output to go into ( grayImage )
         * and what the convertion should be ( in this case we say convert BGR to Gray )
         */
        Cv2.CvtColor(srcImage, grayImage, ColorConversionCodes.BGR2GRAY);

        /**
         * this method normalyse the brigthness and contrast, it is mainly use after converting to grayscale
         * to normalise the contrast and the brightness since it makes the cascade file working better
         */
        Cv2.EqualizeHist(grayImage, grayImage);

// from the classifier ( cascadesFace ) we want to detect as many faces as we can
// and minSize is the minimum size of the detected face( in this example we say that if the classifier find a face
// which is less than 60x60 then we dont want to classifier it as a face so ignore it
// it is sometimes useful because depending on how the background look like, it can happen that some background stuffs
// get classified as a face while not being one
        var faces = _cascadeClassifierFaces.DetectMultiScale(grayImage, minSize: new Size(60, 60));

        Console.WriteLine("we found " + faces.Length + " faces in this image");

        foreach (var faceRect in faces)
            // using var detectedFaceImage = new Mat(srcImage, face);
            /**
             * Drawing a the rectangle that I found ( faceRect ) on the source image
             * with a color and the thickness of the rectangle
             */
            Cv2.Rectangle(srcImage, faceRect, color, 2);

// open up a new window containing the image 
        Cv2.ImShow("faceDetected", srcImage);
        var key = Cv2.WaitKey(); // for killing the window
    }


    public void DetectFacesAndEyes()
    {
        using var newImage = new Mat(@"./Images/faces2.jpg");
        using var grayImage = new Mat();
        var color = Scalar.Green;

        Cv2.CvtColor(newImage, grayImage, ColorConversionCodes.BGR2GRAY);
        Cv2.EqualizeHist(grayImage, grayImage);
        var faces = _cascadeClassifierFaces.DetectMultiScale(
            grayImage,
            minSize: new Size(60, 60));
        Console.WriteLine("Detected Face: {0}", faces.Length);

        foreach (var rect in faces)
        {
            using var detectedFace = new Mat(newImage, rect);
            Cv2.Rectangle(newImage, rect, color, 2);

            using var detectedImageGray = new Mat();
            Cv2.CvtColor(detectedFace, detectedImageGray, ColorConversionCodes.BGR2GRAY);
            Cv2.EqualizeHist(detectedImageGray, detectedImageGray);

            var eyesContainers = _cascadeClassifierEyes.DetectMultiScale(
                detectedImageGray,
                minSize: new Size(5, 5));

            foreach (var eyesRect in eyesContainers)
            {
                /**
                 * calculating the point ( the middle ) of the detected eyes 
                 */
                var center = new Point(
                    (int)(Math.Round(eyesRect.X + eyesRect.Width * 0.5, MidpointRounding.ToEven) +
                          rect.Left),
                    (int)(Math.Round(eyesRect.Y + eyesRect.Height * 0.5, MidpointRounding.ToEven) +
                          rect.Top));

                /**
                 * calculating the radius of the cercle which would be paint around the circle
                 */
                var radius = Math.Round((eyesRect.Width + eyesRect.Height) * 0.25, MidpointRounding.ToEven);
                Cv2.Circle(newImage, center, (int)radius, color, 2);
            }
        }

        Cv2.ImShow("Faces and eyes detection", newImage);
        var key = Cv2.WaitKey();
    }

    public void DetectAndSave(byte[] bytes)
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
    }

    public async Task HttpPostTest(string path)
    {
        using var ms = new MemoryStream();
        using var image = Image.FromFile(path);
        image.Save(ms, ImageFormat.Jpeg);
        var bytes = ms.ToArray();
        const string endPoint = "https://localhost:7199/api/faces";

        using var byteContent = new ByteArrayContent(bytes);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        var response = await _client.PostAsync(endPoint, byteContent);
        var apiResponse = await response.Content.ReadAsStringAsync();
        var faceList = JsonSerializer.Deserialize<List<byte[]>>(apiResponse);

        if (faceList != null)
        {
            for (var i = 0; i < faceList.Count; ++i)
            {
                using var ms2 = new MemoryStream();
                using var img = Image.FromStream(ms2);
                img.Save($"image_{i}", ImageFormat.Jpeg);
            }

            Console.WriteLine("request succeed");
        }
        else
        {
            Console.WriteLine("request failed");
        }
    }
}