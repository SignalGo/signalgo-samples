using System;

namespace SignalGoServerSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Server Connecting...");
                var server = new SignalGo.Server.ServiceManager.ServerProvider();

                server.Start("http://localhost:1199/SignalGoTestServicesProject");
                Console.WriteLine("Server Connected");

                server.AddHttpService(typeof(SimpleHttpRequest));
                server.InitializeService(typeof(SignalGoServerMethods));

                server.RegisterClientCallbackInterfaceService<ISignalGoClientMethods>();
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
