using SignalGo.Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetMVCOWINSample
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