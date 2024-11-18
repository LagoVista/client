using LagoVista.Client.Core.Resources;
using LagoVista.Core.Validation;
using LagoVista.UserAdmin.Models.DTOs;
using System;
using System.Text.RegularExpressions;

namespace LagoVista
{
    public static class ValidationExtensions
    {
        public static InvokeResult Validate(this ChangePassword Model, String confirmPassword)
        {
            if (String.IsNullOrEmpty(Model.OldPassword))
            {
                return ClientResources.ChangePassword_OldPasswordRequired.ToFailedInvokeResult();
            }

            if (String.IsNullOrEmpty(Model.NewPassword))
            {
                ClientResources.ChangePassword_NewPasswordRequired.ToFailedInvokeResult();
            }

            var passwordRegEx = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
            if (!passwordRegEx.Match(Model.NewPassword).Success)
            {
                ClientResources.Password_Requirements.ToFailedInvokeResult();
            }

            if (String.IsNullOrEmpty(confirmPassword))
            {
                ClientResources.ChangePassword_ConfirmNewPassword.ToFailedInvokeResult();
            }

            if (confirmPassword != Model.NewPassword)
            {
                ClientResources.ChangePassword_NewConfirmMatch.ToFailedInvokeResult();
            }

            return InvokeResult.Success;

        }
    }
}
