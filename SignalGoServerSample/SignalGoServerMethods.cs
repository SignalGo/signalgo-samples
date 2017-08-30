using SignalGo.Server.Models;
using SignalGo.Shared.DataTypes;
using SignalGo.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SignalGoServerSample
{
    [ServiceContract("SignalGoTestService")]
    public class SignalGoServerMethods
    {
        public ISignalGoClientMethods callback = null;
        OperationContext currentContext = null;
        public SignalGoServerMethods()
        {
            currentContext = OperationContext.Current;
            callback = currentContext.GetClientCallback<ISignalGoClientMethods>();
            Console.Write("client connected: " + currentContext.Client.IPAddress);
        }

        public bool Login(string userName, string password)
        {
            Console.Write("client called Login: UserName:" + userName + " , Password:" + password);

            //get all clients call interface list
            foreach (var call in currentContext.GetAllClientCallbackList<ISignalGoClientMethods>())
            {
                //call GetMeSignalGo method
                var result = call.GetMeSignalGo("test");

                Console.WriteLine("GetMeSignalGo result: " + result);

                //call HelloSignalGo method
                call.HelloSignalGo("hello signalGo");

                Console.WriteLine("hello signalGo call Success");
            }

            if (userName == "admin" && password == "admin")
                return true;
            return false;
        }

        public StreamInfo DownloadStream(string message)
        {
            Console.WriteLine("client want to download stream: " + message);
            var streamInfo = new StreamInfo() { Headers = new Dictionary<string, object>() { { "Size", 10 } } };
            streamInfo.Stream = new MemoryStream(new byte[] { 3, 2, 1, 4, 5, 6, 7, 9, 10, 8 });
            return streamInfo;
        }

        public void UploadStream(StreamInfo streamInfo)
        {
            var len = int.Parse(streamInfo.Headers["Size"].ToString());
            int readLen = 0;
            while (readLen < len)
            {
                byte[] bytes = new byte[len];
                var readCount = streamInfo.Stream.Read(bytes, 0, bytes.Length);
                readLen += readCount;
                Console.WriteLine("stream uploaded " + readCount + " bytes");
            }
        }
    }
}
