using SignalGo.Shared.DataTypes;
using System.Threading.Tasks;
using SignalGo.Shared.Models;
using System;
using SignalGo.Shared.Http;
using TestClientService.ServerServices;
using TestClientService.HttpServices;
using TestClientService.Models;
using TestClientService.ClientServices;
using TestClientService.Enums;

namespace TestClientService.ServerServices
{
    [ServiceContract("SignalGoTestService",ServiceType.ServerService, InstanceType.SingleInstance)]
    public interface ISignalGoServerMethods
    {
        Tuple<bool> Login(string userName, string password);
        Task<Tuple<bool>> LoginAsync(string userName, string password);
    }
}

namespace TestClientService.StreamServices
{
}

namespace TestClientService.HttpServices
{
    public class SimpleHttpRequest
    {
        ActionResult DownloadImage(string name, int num)
        {
                throw new NotSupportedException();
        }
        Task<ActionResult> DownloadImageAsync(string name, int num)
        {
                return System.Threading.Tasks.Task<ActionResult>.Factory.StartNew(() => throw new NotSupportedException());
        }
        string Hello(string name)
        {
                throw new NotSupportedException();
        }
        Task<string> HelloAsync(string name)
        {
                return System.Threading.Tasks.Task<string>.Factory.StartNew(() => throw new NotSupportedException());
        }
        string TestUploadFile(Guid token, int profileId)
        {
                throw new NotSupportedException();
        }
        Task<string> TestUploadFileAsync(Guid token, int profileId)
        {
                return System.Threading.Tasks.Task<string>.Factory.StartNew(() => throw new NotSupportedException());
        }
    }
}

namespace TestClientService.Models
{
}

namespace TestClientService.ClientServices
{
    [ServiceContract("SignalGoTestClientService",ServiceType.ClientService, InstanceType.SingleInstance)]
    public interface ISignalGoTestClientService
    {
        void HelloSignalGo(string hello);
        Tuple<string> GetMeSignalGo(string value);
    }
}

namespace TestClientService.Enums
{
}

