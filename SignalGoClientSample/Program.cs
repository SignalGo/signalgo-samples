using SignalGo.Client;
using SignalGo.Shared.DataTypes;
using SignalGo.Shared.Models;
using SignalGoSharedSample;
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
            
            
            Console.ReadLine();
        }
    }
}
