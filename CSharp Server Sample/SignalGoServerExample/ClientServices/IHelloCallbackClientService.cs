using SignalGo.Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SignalGoServerExample.ClientServices
{
    [ServiceContract("HelloCallback", ServiceType.ClientService, InstanceType.SingleInstance)]
    public interface IHelloCallbackClientService
    {
        Task ReceivedMessageAsync(string name, string family);
    }
}
