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
                //ExampleToServerConsole();
                ExampleToServerIIS();
            }
            catch (Exception ex)
            {
                Console.WriteLine("cannot connect to server, are you sure you start server side app?");
            }
            Console.ReadKey();
        }

        public static void ExampleToServerConsole()
        {
            ClientProvider clientProvider = new ClientProvider();
            //if you want connect to asp.net or asp core server use this line because those servers just support websocket for duplex connections
            clientProvider.ProtocolType = SignalGo.Client.ClientManager.ClientProtocolType.WebSocket;
            //"asp.net mvc example" server port is 10012
            var service = clientProvider.RegisterServerService<MyTestServices.ServerServices.HelloWorldService>(clientProvider);
            clientProvider.RegisterClientService<MyCallback>();
            clientProvider.Connect("http://localhost:9674/SignalGo");
            //var service = new MyTestServices.HttpServices.HelloWorldService("http://localhost:9674");
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
            var callbackResult = service.CallClientService("ali", "yousefi");
            Console.WriteLine("login called:");
            Console.WriteLine(loginResult);
            Console.WriteLine(callbackResult);
        }

        public static void ExampleToServerIIS()
        {
            ClientProvider clientProvider = new ClientProvider();
            //if you want connect to asp.net or asp core server use this line because those servers just support websocket for duplex connections
            clientProvider.ProtocolType = SignalGo.Client.ClientManager.ClientProtocolType.WebSocket;
            clientProvider.RegisterClientService<MyCallback>();
            //"asp.net mvc example" server port is 10012
            //"asp.net mvc core example" server port is 9674
            var service = clientProvider.RegisterServerService<MyTestServices.ServerServices.HelloWorldService>(clientProvider);
            //var service = new MyTestServices.HttpServices.HelloWorldService("http://localhost:9674");
            clientProvider.Connect("http://localhost:9674/SignalGo");
            var loginResult = service.Login("ali yousefi");
            var helloResult = service.Hello();
            var callbackResult = service.CallClientService("ali", "yousefi");
            Console.WriteLine("login called:");
            Console.WriteLine(loginResult);
            Console.WriteLine(callbackResult);
        }


    }

    public class MyCallback : MyTestServices.ClientServices.IHelloCallbackClientService
    {
        public void ReceivedMessage(string name, string family)
        {
            Console.WriteLine($"callback called name:{name} family:{family}");
        }

        public void ReceivedMessageBase(string name, string family)
        {
            Console.WriteLine($"callback called base name:{name} family:{family}");
        }
    }
}
