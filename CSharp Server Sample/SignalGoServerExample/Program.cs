using SignalGo.Server.ServiceManager;
using System;

namespace SignalGoServerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerProvider serverProvider = new ServerProvider();
            serverProvider.RegisterServerService<Services.HelloWorldService>();
            serverProvider.RegisterClientService<ClientServices.IHelloCallbackClientService>();
            //to handle cross origin errors
            serverProvider.ProviderSetting.HttpSetting.HandleCrossOriginAccess = true;
            serverProvider.Start("http://localhost:9674/SignalGo");
            Console.WriteLine("server started");
            Console.ReadKey();
        }
    }
}
