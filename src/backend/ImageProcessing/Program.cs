using ImageProcessing;

var demo = new DemoFaceDetection();
// demo.DetectFacesAndEyes();

const string imagePath = @"./Images/faces5.jpg";

demo?.HttpPostTest(imagePath);


// byte[] CreateByteArrayFromImage(string path)
// {
//     using var image = Image.FromFile(path);
//     using var ms = new MemoryStream();
//     
//     image.Save(ms, ImageFormat.Jpeg);
//     var result = ms.ToArray();
//     return result;
// }
//
// byte[] CreateByteArrayFromPath(string path)
// {
//     using var stream = new FileStream(path, FileMode.Open);
//     using var ms = new MemoryStream();
//     
//     stream.CopyTo(ms);
//
//     var result = ms.ToArray();
//     return result;
// }