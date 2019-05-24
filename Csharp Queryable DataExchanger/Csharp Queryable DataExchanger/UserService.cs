using Csharp_Queryable_DataExchanger.Models;
using SignalGo.DataExchanger.Compilers;
using SignalGo.Shared.DataTypes;
using System.Collections.Generic;
using System.Linq;

namespace Csharp_Queryable_DataExchanger
{
    [ServiceContract("User", ServiceType.HttpService, InstanceType.SingleInstance)]
    public class UserService
    {
        public List<UserInfo> GetAllUsers(string query)
        {
            var queryFromDataBaseExample = Program.InitializeUsers();

            SelectCompiler selectCompiler = new SelectCompiler();
            string anotherResult = selectCompiler.Compile(query);
            ConditionsCompiler conditionsCompiler = new ConditionsCompiler();
            conditionsCompiler.Compile(anotherResult);

            object result = selectCompiler.Run(queryFromDataBaseExample);
            IEnumerable<UserInfo> resultWheres = (IEnumerable<UserInfo>)conditionsCompiler.Run<UserInfo>(queryFromDataBaseExample);
            List<UserInfo> resultData = resultWheres.ToList();
            return resultData;
        }
    }
}
