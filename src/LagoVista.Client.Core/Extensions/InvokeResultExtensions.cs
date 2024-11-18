using LagoVista.Core.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista
{
    public static class InvokeResultExtensions
    {

        public static ErrorMessage ToErrorMessage(this string contents, string errorCode = "", bool systemError = true)
        {
            return new ErrorMessage()
            {
                Message = contents,
                ErrorCode = errorCode,
                SystemError = systemError
            };
        }

        public static InvokeResult ToFailedInvokeResult(this string contents, string errorCode = "", bool systemError = true)
        {
            var result =  InvokeResult.FromErrors(new ErrorMessage()
            {
                Message = contents,
                ErrorCode = errorCode,
                SystemError = systemError
            });

            return result;
        }

        public static KeyValuePair<string, string> ToKVP(this string value, string key)
        {
            return new KeyValuePair<string, string>(key, value);
        }
    }
}
