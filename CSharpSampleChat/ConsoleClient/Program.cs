using ServerServices.ClientServices;
using ServerServices.ServerServices;
using SignalGo.Client;
using System;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientProvider clientProvider = new ClientProvider();
            var chatService  = clientProvider.RegisterServerService<ChatService>();
            clientProvider.RegisterClientService<MyChatService>();
            clientProvider.Connect("http://localhost:6262/any");
            Console.WriteLine("Client Connected!");
            Console.WriteLine("write a message and press enter to send to all clients");
            var message = Console.ReadLine();
            var result = chatService.SendToAll(message);
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }

    public class MyChatService : IClientChatService
    {
        public void OnMessage(string message)
        {
            Console.WriteLine($"Message Received {message}");
        }
    }
}
