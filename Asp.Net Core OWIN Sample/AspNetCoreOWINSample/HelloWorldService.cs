using SignalGo.Shared.DataTypes;
using System;

namespace AspNetCoreOWINSample
{
    [ServiceContract("HelloWorld", ServiceType.HttpService, InstanceType.SingleInstance)]
    public class HelloWorldService
    {
        public string Hello()
        {
            return "Hello SignalGo! " + DateTime.Now;
        }
    }
}
