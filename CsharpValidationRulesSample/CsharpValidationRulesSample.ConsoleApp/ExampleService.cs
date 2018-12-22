using SignalGo.Shared.DataTypes;

namespace CsharpValidationRulesSample.ConsoleApp
{
    [ServiceContract("Example", ServiceType.HttpService, InstanceType.SingleInstance)]
    public class ExampleService
    {
        /// <summary>
        /// login method with validations of parameters
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Login([EmptyValidation]string userName, [EmptyValidation] string password)
        {
            return $"method called,validation is ok userName:{userName} password:{password}";
        }

        /// <summary>
        /// add contact method with flaut api validation in program.cs file
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public string AddContact(Contact contact)
        {
            return $"add contact success called with name : {contact.Name}";
        }
    }
}
