using SignalGo.Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalGoServerSample
{
    [ServiceContract("SignalGoTestClientService")]
    public interface ISignalGoClientMethods
    {
        void HelloSignalGo(string hello);
        string GetMeSignalGo(string value);
    }
}
