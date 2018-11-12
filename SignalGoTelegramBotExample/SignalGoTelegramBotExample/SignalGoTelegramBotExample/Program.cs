using SignalGo.Server.ServiceManager;
using SignalGo.Server.TelegramBot;
using SignalGo.Shared.DataTypes;
using System;
using System.Collections.Generic;

namespace SignalGoTelegramBotExample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ServerProvider serverProvider = new ServerProvider();
            serverProvider.RegisterServerService<AuthenticationService>();
            serverProvider.RegisterServerService<BookService>();
            serverProvider.ProviderSetting.IsEnabledDataExchanger = false;
            serverProvider.Start("http://localhost:6452/SignalGoTest/any");

            SignalGoBotManager signalGoBotManager = new SignalGoBotManager();
            signalGoBotManager.Start("your telegram bot token here", serverProvider);
            Console.WriteLine("server started successfuly call your services with telegram bot or http request, example http://localhost:6452/Book/GetListOfBook");
            Console.ReadLine();
        }
    }

    [ServiceContract("Authentication", ServiceType.HttpService, InstanceType = InstanceType.SingleInstance)]
    public class AuthenticationService
    {
        public string Login(string userName, string password)
        {
            if (userName == "admin" && password == "admin")
                return "login success";
            return "username or password incorrect!";
        }

        public string Register(string name, string family)
        {
            return $"Welcome to signalgo test {name} {family}";
        }
    }

    [ServiceContract("Book", ServiceType.HttpService, InstanceType = InstanceType.SingleInstance)]
    public class BookService
    {
        public List<Book> GetListOfBook()
        {
            return new List<Book>()
            {
                new Book() { Name = "God" , PagesCount = 1500 },
                new Book() { Name = "SignalGo" , PagesCount = 2000 },
            };
        }
    }

    public class Book
    {
        public string Name { get; set; }
        public int PagesCount { get; set; }
    }
}
