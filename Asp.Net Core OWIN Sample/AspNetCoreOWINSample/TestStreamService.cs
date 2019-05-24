using SignalGo.Server.Models;
using SignalGo.Shared.DataTypes;
using SignalGo.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreOWINSample
{
    [ServiceContract("TestStream", ServiceType.HttpService, InstanceType.SingleInstance)]
    public class TestStreamService
    {
        public StreamInfo<DateTime> DownloadProfileImage(int userId, string filePath)
        {
            //do somthing
            var stream = new StreamInfo<DateTime>();
            if (!File.Exists(filePath))
            {
                //return error when want for example file not found
                stream.Status = System.Net.HttpStatusCode.NotFound;
                return stream;
            }

            var file = new System.IO.FileInfo(filePath);
            stream.Stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            //length is important
            stream.Length = file.Length;
            //your result data
            stream.Data = file.LastWriteTime;

            OperationContext.Current.HttpClient.ResponseHeaders.Add("content-disposition", ("attachment; filename=" + file.Name).Split(','));
            OperationContext.Current.HttpClient.ResponseHeaders.Add("Content-Type", file.Extension);

            return stream;
        }

        public string UploadProfileImage(string fileName, StreamInfo<string> streamInfo)
        {
            //you can use OperationContext<T> to get your client setting with client id if you dont have client plan ignore it (you can read about OperationContext in wiki too)
            //var currentUserSetting = OperationContext<YourSettingClass>.GetCurrent(streamInfo.ClientId);
            string filePath = "your file path to save";

            //this is an example to read stream and save to file
            using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                var lengthWrite = 0;
                while (lengthWrite != streamInfo.Length)
                {
                    byte[] bytes = new byte[1024];
                    var readCount = streamInfo.Stream.Read(bytes, bytes.Length);
                    if (readCount <= 0)
                        break;
                    fileStream.Write(bytes, 0, readCount);
                    lengthWrite += readCount;
                    //if you have a progress bar in client side this code will send your server position to client and client can position it if you don't have progressbar just pervent this line
                    streamInfo.SetPositionFlushAsync(lengthWrite);
                }
            }

            //make your custom result
            //return MessageContract.Success();
            return "success";
        }
    }
}
