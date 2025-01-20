using HolbertonCRM.Utilities.ConstantMessages;

namespace HolbertonCRM.Utilities.Helpers
{
    public class EmailBodyHelper
    {
        private readonly IFileService _fileService;

        public EmailBodyHelper(IFileService fileService)
        {
            _fileService = fileService;
        }

        public static string PrepareBody(string templatePath, string subject, string link = null, string otp = null)
        {
            string body = _fileService.ReadFile(templatePath, body);

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
