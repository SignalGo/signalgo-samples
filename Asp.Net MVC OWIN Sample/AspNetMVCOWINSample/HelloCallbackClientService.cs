using SignalGo.Shared.DataTypes;
using System.Threading.Tasks;

namespace AspNetMVCOWINSample
{
    [ServiceContract("HelloCallback", ServiceType.ClientService, InstanceType.SingleInstance)]
    public interface IHelloCallbackClientService
    {
        Task ReceivedMessageAsync(string name, string family);
    }
}