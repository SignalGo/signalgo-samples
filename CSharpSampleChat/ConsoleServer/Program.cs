using SignalGo.Server.Models;
using SignalGo.Server.ServiceManager;
using SignalGo.Shared.DataTypes;
using System;

namespace ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerProvider serverProvider = new ServerProvider();
            serverProvider.RegisterServerService<ChatService>();
            serverProvider.RegisterClientService<IClientChatService>();
            serverProvider.Start("http://localhost:6262/any");
            Console.WriteLine("server started");
            Console.ReadLine();
        }
    }

    [ServiceContract("Chat", ServiceType.ServerService, InstanceType.SingleInstance)]
    public class ChatService
    {
        public string SendToAll(string message)
        {
            foreach (var service in OperationContext.Current.GetAllClientServices<IClientChatService>())
            {
                service.OnMessage(message);
            }
            return "Messages sent";
        }
    }

    [ServiceContract("ClientChat", ServiceType.ClientService, InstanceType.SingleInstance)]
    public interface IClientChatService
    {
        void OnMessage(string message);
    }
}
