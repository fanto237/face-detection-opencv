// using System.Net.Http.Headers;
//
// namespace ImageProcessing;
//
// public class FaceApiTest
// {
//     void ApiTest()
//     {
//         const string imagePath = @"kadyn-pierce-L203i9Xi_XE-unsplash.jpg";
//         const string urlAddress = "http://localhost:6001/api/faces";
//         // var bytes = ImageUtility.ConvertToBytes(imagePath);
//         byte[] bytes = null;
//         List<byte[]> faceList;
//         var byteContent = new ByteArrayContent(bytes);
//         byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
//         using (var httpClient = new HttpClient())
//         {
//             using (var response = await httpClient.PostAsync(urlAddress, byteContent))
//             {
//                 var apiResponse = await response.Content.ReadAsStringAsync();
//                 // faceList = JsonConvert.DeserializeObject<List<byte[]>>(apiResponse);
//             }
//         }
//
//         if (faceList is { Count: > 0 })
//             for (var i = 0; i < faceList.Count; i++)
//                 // ImageUtility.FromBytesToImage(faceList[i], "face" + i);
//     }
// }

