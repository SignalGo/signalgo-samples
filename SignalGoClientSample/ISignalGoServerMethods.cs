using SignalGo.Shared.DataTypes;
using SignalGo.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalGoClientSample
{
    [ServiceContract("SignalGoTestService")]
    public interface ISignalGoServerMethods
    {
        bool Login(string userName, string password);
        StreamInfo DownloadStream(string message);
        void UploadStream(StreamInfo streamInfo);
    }
}
