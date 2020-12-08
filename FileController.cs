using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace TestWebApiApp.Controllers
{
    public class FileController : ApiController
    {
        /// <summary>
        /// API to get input file and upload to qrserver and get response.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>client response</returns>
        [HttpGet]
        [Route("api/File/UploadFile/{*filePath}")]
        public string UploadFile([FromUri]string filePath)
        {
            using (var client = new HttpClient())
            {
                using (var multipartFormDataContent = new MultipartFormDataContent())
                {
                    FileInfo file = new FileInfo(filePath);
                    byte[] fileData = System.IO.File.ReadAllBytes(file.ToString());
                    multipartFormDataContent.Add(new StreamContent(new MemoryStream(fileData)), "file", "TestQR");
                    var response = client.PostAsync("http://api.qrserver.com/v1/read-qr-code/", multipartFormDataContent).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        return "Exception Message.";
                    }

                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }

        /// <summary>
        /// Method is used to read file from local folder app_data and uload to qrserver.
        /// </summary>
        /// <returns>client response</returns>
        [HttpGet]
        [Route("api/File/FileUpload")]
        public string FileUpload()
        {
            using (var client = new HttpClient())
            {
                using (var multipartFormDataContent = new MultipartFormDataContent())
                {
                    byte[] fileData = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/App_Data/TestQR.png"));
                    multipartFormDataContent.Add(new StreamContent(new MemoryStream(fileData)), "file", "TestQR");
                    var response = client.PostAsync("http://api.qrserver.com/v1/read-qr-code/", multipartFormDataContent).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        return "Exception Message.";
                    }

                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }
    }
}
