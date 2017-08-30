using SignalGo.Shared.DataTypes;
using SignalGo.Shared.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SignalGoServerSample
{
    [HttpSupport("AddressTest")]
    public class SimpleHttpRequest : HttpRequestController
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
            if (num <= 0)
            {
                Status = System.Net.HttpStatusCode.Forbidden;
                return Content("num is not true!");
            }
            ResponseHeaders.Add("Content-Type", "image/jpeg");
            return new FileActionResult(@"D:\hamed.jpg");
        }
        /// <summary>
        /// test url example on your browser:
        /// http://localhost:1199/AddressTest/Hello?name=ali
        /// </summary>
        /// <param name="name"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public ActionResult Hello(string name)
        {
            return Content("hello:" + name);
        }

        public ActionResult TestUploadFile(Guid token, int profileId)
        {
            var fileInfo = TakeNextFile();
            if (fileInfo == null)
            {
                Status = System.Net.HttpStatusCode.NotFound;
                return Content("file not found!");
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
            return Content("success!");
        }
    }
}
