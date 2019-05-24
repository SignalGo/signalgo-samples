using SignalGo.Server.Models;
using SignalGo.Shared.DataTypes;
using System;
using System.Threading.Tasks;

namespace AspNetCoreOWINSample
{
    [ServiceContract("HelloWorld", ServiceType.HttpService, InstanceType.SingleInstance)]
    [ServiceContract("HelloWorld", ServiceType.ServerService, InstanceType.SingleInstance)]
    public class HelloWorldService
    {
        /// <summary>
        /// simple method test
        /// </summary>
        /// <param name="yourName"></param>
        /// <returns></returns>
        public string Login(string yourName)
        {
            return $"Hello {yourName} welcome to signalgo...";
        }
        
        public string Hello()
        {
            return "Hello SignalGo! " + DateTime.Now;
        }

        private bool PrivateMethod()
        {
            return true;
        }

        public async Task<string> LoginTimmeMethod()
        {
            await Task.Delay(1000 * 60 * 5);
            return await Task.FromResult("this is long time result");
        }

        public async Task<string> CallClientService(string name, string family)
        {
            // call clients methods
            foreach (ClientContext<IHelloCallbackClientService> item in OperationContext.Current.GetAllClientClientContextServices<IHelloCallbackClientService>())
            {
                await item.Service.ReceivedMessageAsync(name, family);
            }
            return name + " " + family;
        }
    }
}
