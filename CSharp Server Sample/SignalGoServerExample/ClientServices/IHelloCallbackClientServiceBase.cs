using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SignalGoServerExample.ClientServices
{
    /// <summary>
    /// example of base interface
    /// </summary>
    public interface IHelloCallbackClientServiceBase
    {
        Task ReceivedMessageBaseAsync(string name, string family);
    }
}
