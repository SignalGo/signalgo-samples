using SignalGo.Client;
using SignalGo.Shared.DataTypes;
using SignalGo.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestClientService.ServerServices;

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

            var callbacks = connector.RegisterClientService<ClientCallback>();

            var service = connector.RegisterServerServiceInterfaceWrapper<ISignalGoServerMethods>();
            Console.WriteLine("Client Register Comeplete.");

            var result = service.Login("admin", "admin");

            Console.WriteLine("Login Result: " + result.Item1);
            
            
            Console.ReadLine();
        }
    }
}
