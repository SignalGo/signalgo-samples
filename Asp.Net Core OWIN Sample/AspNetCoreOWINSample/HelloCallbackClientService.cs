using SignalGo.Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreOWINSample
{
    [ServiceContract("HelloCallback", ServiceType.ClientService, InstanceType.SingleInstance)]
    public interface IHelloCallbackClientService
    {
        Task ReceivedMessageAsync(string name, string family);
    }
}
