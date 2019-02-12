using SignalGo.Server.Models;
using SignalGo.Shared.DataTypes;
using System;
using System.Threading.Tasks;

namespace AspNetMVCOWINSample
{
    [ServiceContract("HelloWorld", ServiceType.HttpService, InstanceType.SingleInstance)]
    [ServiceContract("HelloWorld", ServiceType.ServerService, InstanceType.SingleInstance)]
    public class HelloWorldService
    {
        public string Hello()
        {
            return "Hello SignalGo! " + DateTime.Now;
        }

        public async Task<string> CallClientService(string name, string family)
        {
            //call clients methods
            foreach (ClientContext<IHelloCallbackClientService> item in OperationContext.Current.GetAllClientClientContextServices<IHelloCallbackClientService>())
            {
                await item.Service.ReceivedMessageAsync(name, family);
            }
            return name + " " + family;
        }
    }
}