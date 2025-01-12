using HolbertonCRM.Utilities.ConstantMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolbertonCRM.Utilities.Helpers
{
    public class EmailBodyHelper
    {
        public static string PrepareBody(string templatePath, string subject, string link = null, string otp = null)
        {
            string body = File.ReadAllText(templatePath);

            switch (subject.ToLower())
            {
                case "resetpassword":
                    body = body.Replace("{{action_url}}", link);
                    body = body.Replace("{{email_title}}", AuthMessages.ResetPasswordTitle);
                    body = body.Replace("{{message}}", AuthMessages.ResetPasswordMessage);
                    break;

                case "verify":
                    body = body.Replace("{{action_url}}", string.Empty);
                    body = body.Replace("{{email_title}}", AuthMessages.VerifyEmailTitle);
                    body = body.Replace("{{message}}", AuthMessages.VerifyEmailMessage + otp);
                    break;

                default:
                    body = body.Replace("{{action_url}}", string.Empty);
                    body = body.Replace("{{email_title}}", AuthMessages.DefaultEmailTitle);
                    body = body.Replace("{{message}}", AuthMessages.DefaultEmailMessage);
                    break;
            }

            return body;
        }
    }
}
