using SignalGo.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpClientSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ClientProvider clientProvider = new ClientProvider();
                //if you want connect to asp.net or asp core server use this line because those servers just support websocket for duplex connections
                clientProvider.ProtocolType = SignalGo.Client.ClientManager.ClientProtocolType.WebSocket;
                //"asp.net mvc example" server port is 10012
                clientProvider.Connect("http://localhost:9674/SignalGo");
                //var service = clientProvider.RegisterServerService<MyTestServices.ServerServices.HelloWorldService>(clientProvider);
                var service = new MyTestServices.HttpServices.HelloWorldService("http://localhost:9674");
                var streamService = new MyTestServices.HttpServices.TestStreamService("http://localhost:9674");

                var downloadImage = streamService.DownloadProfileImage(1, "H:\\json.txt");
                var length = int.Parse(streamService.ResponseHeaders["content-length"]);
                var bytes = new List<byte>();
                int pos = 0;
                while (pos != length)
                {
                    var readb = new byte[length - pos];
                    var readCount = downloadImage.Stream.Read(readb, length - pos);
                    pos += readCount;
                    bytes.AddRange(readb.ToList().GetRange(0, readCount));
                }
                var fileData = Encoding.UTF8.GetString(bytes.ToArray());
                var loginResult = service.Login("ali yousefi");
                Console.WriteLine("login called:");
                Console.WriteLine(loginResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine("cannot connect to server, are you sure you start server side app?");
            }
            Console.ReadKey();
        }
    }
}
