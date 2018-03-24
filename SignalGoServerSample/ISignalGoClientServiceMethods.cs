using SignalGo.Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalGoServerSample
{
    [ServiceContract("SignalGoTestClientService", ServiceType.ClientService, InstanceType.SingleInstance)]
    public interface ISignalGoClientServiceMethods
    {
        void HelloSignalGo(string hello);
        Tuple<string> GetMeSignalGo(string value);
    }
}
