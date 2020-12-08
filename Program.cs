using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleAppQR
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.Write("Please enter file path: ");
            string filePath = Console.ReadLine();

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
                        Console.WriteLine("Exception Message.");
                    }

                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                }
            }

            Console.ReadKey();
        }
    }
}
