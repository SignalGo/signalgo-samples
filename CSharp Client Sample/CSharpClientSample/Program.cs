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
                clientProvider.Connect("http://localhost:6428/SignalGo");
                var service = clientProvider.RegisterServerService<MyTestServices.ServerServices.HelloWorldService>(clientProvider);
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
