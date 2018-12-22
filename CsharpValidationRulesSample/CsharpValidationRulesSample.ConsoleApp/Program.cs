using SignalGo.Server.ServiceManager;
using SignalGo.Shared.DataTypes;
using SignalGo.Shared.Models;
using System;
using System.Diagnostics;
using System.Text;

namespace CsharpValidationRulesSample.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //create instace of server provider
            ServerProvider serverProvider = new ServerProvider();
            //register your service class
            serverProvider.RegisterServerService<ExampleService>();

            //customize your validation resutl to client
            serverProvider.ValidationResultHandlingFunction = (validations, service, method) =>
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (BaseValidationRuleInfoAttribute item in validations)
                {
                    stringBuilder.AppendLine(BaseValidationRuleInfoAttribute.GetErrorValue(item).ToString());
                }
                return stringBuilder.ToString();
            };

            //start server with port 4521
            serverProvider.Start("http://localhost:4521/any");

            InitializeValidationRules(serverProvider);


            //example start in your browser

            //invalid login
            StartProcess("http://localhost:4521/Example/Login?userName=&password=");
            //valid login
            StartProcess("http://localhost:4521/Example/Login?userName=test&password=testpass");
            //valid addcontact
            StartProcess("http://localhost:4521/Example/AddContact?contact=" + Uri.EscapeDataString("{\"Name\":\"test\"}"));
            //invalid addcontact
            StartProcess("http://localhost:4521/Example/AddContact?contact={}");
            Console.WriteLine("Server started!");
            Console.ReadLine();
        }

        private static void StartProcess(string url)
        {
            url = url.Replace("&", "^&");
            //url = url.Replace("\"", "\"\"");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }

        /// <summary>
        /// initialize flaut api validations
        /// </summary>
        /// <param name="serverBase"></param>
        public static void InitializeValidationRules(ServerBase serverBase)
        {
            //add EmptyValidation for Contact class for Email property
            typeof(Contact).Validation(serverBase)
                .Add<EmptyValidationAttribute>("Name")
                .Build();
        }
    }
}
