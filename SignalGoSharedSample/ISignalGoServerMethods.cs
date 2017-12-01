using SignalGo.Shared.DataTypes;
using SignalGo.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalGoSharedSample
{
    [ServiceContract("SignalGoTestService", InstanceType.SingleInstance)]
    public interface ISignalGoServerMethods
    {
        Tuple<bool> Login(string userName, string password);
    }
}
