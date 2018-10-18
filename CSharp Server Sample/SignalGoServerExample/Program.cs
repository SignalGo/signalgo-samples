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
            //to handle cross origin errors
            serverProvider.ProviderSetting.HttpSetting.HandleCrossOriginAccess = true;
            serverProvider.Start("http://localhost:6428/SignalGo");
            Console.WriteLine("server started");
            Console.ReadKey();
        }
    }
}
