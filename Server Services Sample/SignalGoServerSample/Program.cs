using SignalGo.Server.Settings;
using System;

namespace SignalGoServerSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                
                Console.WriteLine("enable log system...");
                SignalGo.Server.Log.ServerMethodCallsLogger logger = new SignalGo.Server.Log.ServerMethodCallsLogger();
                logger.IsPersianDateLog = true;
                logger.Initialize();
                Console.WriteLine("Server Connecting...");


                var server = new SignalGo.Server.ServiceManager.ServerProvider();

                server.Start("http://localhost:1199/SignalGoTestServicesProject");
                Console.WriteLine("Server Connected");

                server.RegisterServerService<SimpleHttpRequest>();
                server.RegisterServerService<SignalGoServerMethods>();

                server.RegisterClientService<ISignalGoClientServiceMethods>();

                Console.WriteLine("handle cross origin access...");
                server.HttpProtocolSetting = new HttpProtocolSetting() { HandleCrossOriginAccess = true };
                Console.WriteLine("Waiting for Client...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: ");
                Console.WriteLine(ex);
            }
        }
    }
}
