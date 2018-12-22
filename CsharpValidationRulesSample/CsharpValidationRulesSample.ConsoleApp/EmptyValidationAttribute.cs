using SignalGo.Shared.DataTypes;
using System;

namespace CsharpValidationRulesSample.ConsoleApp
{
    public class EmptyValidationAttribute : ValidationRuleInfoAttribute
    {
        /// <summary>
        /// check value is validate
        /// if you return false value is not true so GetErrorValue will call from signalgo and return result to client before call methods
        /// </summary>
        /// <returns></returns>
        public override bool CheckIsValidate()
        {
            if (CurrentValue == null || (CurrentValue is string text && string.IsNullOrEmpty(text.Trim())))
                return false;
            return true;
        }

        public override object GetChangedValue()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// if value is not valid this method will call and take value from you to return to client
        /// </summary>
        /// <returns></returns>
        public override object GetErrorValue()
        {
            //if value is property
            if (PropertyInfo != null)
                return $"please fill {PropertyInfo.Name}";
            //if value is method parameter
            return $"please fill {ParameterInfo.Name}";
        }
    }
}
