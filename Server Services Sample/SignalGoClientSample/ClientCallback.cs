using SignalGo.Client;
using SignalGo.Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalGoClientSample
{
    public class ClientCallback : TestClientService.ClientServices.ISignalGoTestClientService
    {
        public Tuple<string> GetMeSignalGo(string value)
        {
            Console.WriteLine("called GetMeSignalGo: " + value);
            return new Tuple<string>("GetMeSignalGo :" + value);
        }

        public void HelloSignalGo(string hello)
        {
            Console.WriteLine("called HelloSignalGo: " + hello);
        }
    }
}
