using SignalGo.Server.Models;
using SignalGo.Shared.DataTypes;
using SignalGoServerExample.ClientServices;
using SignalGoServerExample.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalGoServerExample.Services
{
    [ServiceContract("HelloWorld", ServiceType.HttpService, InstanceType.SingleInstance)]
    [ServiceContract("HelloWorld", ServiceType.ServerService, InstanceType.SingleInstance)]
    public class HelloWorldService
    {
        /// <summary>
        /// simple method test
        /// </summary>
        /// <param name="yourName"></param>
        /// <returns></returns>
        public string Login(string yourName)
        {
            return $"Hello {yourName} welcome to signalgo...";
        }

        public async Task<string> LoginTimeMethod()
        {
            await Task.Delay(1000 * 5);
            return await Task.FromResult("this is long time result");
        }

        /// <summary>
        /// test to get complex object
        /// </summary>
        /// <returns></returns>
        public List<UserInfo> GetUserInfoes()
        {
            UserInfo user1 = new UserInfo()
            {
                Id = 1,
                Age = 28,
                Name = "Ali",
            };
            user1.Books = new List<BookInfo>()
            {
                new BookInfo()
                {
                     Id = 1,
                     Details = "book 1",
                     UserInfo = user1
                },
                new BookInfo()
                {
                     Id = 2,
                     Details = "book 2",
                     UserInfo = user1
                },
            };

            UserInfo user2 = new UserInfo()
            {
                Id = 2,
                Age = 30,
                Name = "Gerardo",
            };
            user2.Books = new List<BookInfo>()
            {
                new BookInfo()
                {
                     Id = 3,
                     Details = "book 3",
                     UserInfo = user2
                },
                new BookInfo()
                {
                     Id = 4,
                     Details = "book 4",
                     UserInfo = user2
                },
            };
            return new List<UserInfo>() { user1, user2 };
        }

        public string Hello()
        {
            return "Hello SignalGo! " + DateTime.Now;
        }

        public async Task<string> CallClientService(string name, string family)
        {
            // call clients methods
            foreach (ClientContext<IHelloCallbackClientService> item in OperationContext.Current.GetAllClientClientContextServices<IHelloCallbackClientService>())
            {
                if (item.Client.ProtocolType == ClientProtocolType.WebSocket || item.Client.ProtocolType == ClientProtocolType.SignalGoDuplex)
                {
                    await item.Service.ReceivedMessageAsync(name, family);
                    await item.Service.ReceivedMessageBaseAsync(name, family);
                }
            }
            return name + " " + family;
        }
    }
}
