using SignalGo.Client;
using SignalGo.Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalGoClientSample
{
    [ServiceContract("SignalGoTestClientService")]
    public class ClientCallback
    {
        public ConnectorBase Connector
        {
            get
            {
                return SignalGo.Client.OperationContract.GetConnector<ConnectorBase>(this);
            }
        }

        public string GetMeSignalGo(string value)
        {
            Console.WriteLine("called GetMeSignalGo: " + value);
            return "GetMeSignalGo :" + value;
        }

        public void HelloSignalGo(string hello)
        {
            Console.WriteLine("called HelloSignalGo: " + hello);
        }
    }
}
