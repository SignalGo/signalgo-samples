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
                SignalGo.Shared.Log.MethodCallsLogger.IsEnabled = true;

                Console.WriteLine("Server Connecting...");


                var server = new SignalGo.Server.ServiceManager.ServerProvider();

                server.Start("http://localhost:1199/SignalGoTestServicesProject");
                Console.WriteLine("Server Connected");

                server.AddHttpService(typeof(SimpleHttpRequest));
                server.InitializeService(typeof(SignalGoServerMethods));

                server.RegisterClientCallbackInterfaceService<ISignalGoClientMethods>();

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
