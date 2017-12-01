using SignalGo.Client;
using SignalGo.Shared.DataTypes;
using SignalGoSharedSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalGoClientSample
{
    public class ClientCallback : ISignalGoClientMethods
    {
        public ConnectorBase Connector
        {
            get
            {
                return SignalGo.Client.OperationContract.GetConnector<ConnectorBase>(this);
            }
        }

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
