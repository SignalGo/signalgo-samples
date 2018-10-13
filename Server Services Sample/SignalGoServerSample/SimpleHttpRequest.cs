using SignalGo.Server.Models;
using SignalGo.Shared.DataTypes;
using SignalGo.Shared.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SignalGoServerSample
{
    [ServiceContract("AddressTest", ServiceType.HttpService, InstanceType.SingleInstance)]
    public class SimpleHttpRequest
    {
        /// <summary>
        /// test url example on your browser:
        /// http://localhost:1199/AddressTest/DownloadImage?name=ali&num=0
        /// http://localhost:1199/AddressTest/DownloadImage?name=ali&num=10
        /// </summary>
        /// <param name="name"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public ActionResult DownloadImage(string name, int num)
        {
            var current = OperationContext.Current.HttpClient;
            if (num <= 0)
            {
                current.Status = System.Net.HttpStatusCode.Forbidden;
                return "num is not true!";
            }
            current.ResponseHeaders.Add("Content-Type", "image/jpeg");
            //your file address
            string fileName = @"D:\ali.jpg";
            if (!File.Exists(fileName))
            {
                current.Status = System.Net.HttpStatusCode.NotFound;
                return "File not found!";
            }
            return new FileActionResult(fileName);
        }

        /// <summary>
        /// test url example on your browser:
        /// http://localhost:1199/AddressTest/Hello?name=ali
        /// </summary>
        /// <param name="name"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public string Hello(string name)
        {
            return "hello:" + name;
        }

        public string TestUploadFile(Guid token, int profileId)
        {
            var current = OperationContext.Current.HttpClient;
            var fileInfo = current.TakeNextFile();
            if (fileInfo == null)
            {
                current.Status = System.Net.HttpStatusCode.NotFound;
                return "file not found!";
            }
            using (var streamWriter = new FileStream("D:\\testfileName.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                var bytes = new byte[1024 * 10];
                while (true)
                {
                    var readCount = fileInfo.InputStream.Read(bytes, 0, bytes.Length);
                    if (readCount <= 0)
                        break;
                    streamWriter.Write(bytes, 0, readCount);
                }
                long fileLen = streamWriter.Length;
            }
            return "success!";
        }
    }
}
