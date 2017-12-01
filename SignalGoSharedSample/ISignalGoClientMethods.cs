using SignalGo.Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalGoSharedSample
{
    [ServiceContract("SignalGoTestClientService")]
    public interface ISignalGoClientMethods
    {
        void HelloSignalGo(string hello);
        Tuple<string> GetMeSignalGo(string value);
    }
}
