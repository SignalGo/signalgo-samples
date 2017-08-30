using SignalGo.Client;
using SignalGo.Shared.DataTypes;
using SignalGo.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalGoClientSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Connecting...");
            ClientProvider connector = new ClientProvider();
            connector.Connect("http://localhost:1199/SignalGoTestServicesProject");
            Console.WriteLine("Connect Comeplete.");

            var callbacks = connector.RegisterServerCallback<ClientCallback>();

            var service = connector.RegisterClientServiceInterface<ISignalGoServerMethods>();
            Console.WriteLine("Client Register Comeplete.");

            var result = service.Login("admin", "admin");

            Console.WriteLine("Login Result: " + result);

            //download stream from server
            using (var streamInfo = service.DownloadStream("hello stream"))
            {
                var len = int.Parse(streamInfo.Headers["Size"].ToString());
                int readLen = 0;
                while (readLen < len)
                {
                    byte[] bytes = new byte[len];
                    var readCount = streamInfo.Stream.Read(bytes, 0, bytes.Length);
                    readLen += readCount;
                    Console.WriteLine("stream downloaded " + readCount + " bytes");
                }
            }

            //upload stream to server
            using (StreamInfo stream = new StreamInfo() { Headers = new Dictionary<string, object>() { { "Size", 10 } } })
            {
                service.UploadStream(stream);
                var bytesToUpload = new byte[] { 1, 2, 3, 4, 5, 10, 6, 7, 8, 9 };
                stream.Stream.Write(bytesToUpload, 0, bytesToUpload.Length);
                Console.WriteLine("stream uploaded " + bytesToUpload.Length + " bytes");
            }

            Console.ReadLine();
        }
    }

    [ServiceContract("SignalGoTestClientService")]
    public class ClientCallback
    {
        public ConnectorBase Connector
        {
            get
            {
                return SignalGo.Client.OperationContract.GetConnector<ConnectorBase>(this);
            }
        }

        public string GetMeSignalGo(string value)
        {
            Console.WriteLine("called GetMeSignalGo: " + value);
            return "GetMeSignalGo :" + value;
        }

        public void HelloSignalGo(string hello)
        {
            Console.WriteLine("called HelloSignalGo: " + hello);
        }
    }

    [ServiceContract("SignalGoTestService")]
    public interface ISignalGoServerMethods
    {
        bool Login(string userName, string password);
        StreamInfo DownloadStream(string message);
        void UploadStream(StreamInfo streamInfo);
    }
}
