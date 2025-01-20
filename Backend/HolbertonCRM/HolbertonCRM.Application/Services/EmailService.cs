using HolbertonCRM.Application.Interfaces;
using HolbertonCRM.Domain.Models;
using HolbertonCRM.Helpers;
using HolbertonCRM.Utilities.Helpers;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;


namespace HolbertonCRM.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IFileService _fileService;
        string? SMTP_FROM_ADDRESS = Environment.GetEnvironmentVariable("SMTP_FROM_ADDRESS");
        string? SMTP_SERVER = Environment.GetEnvironmentVariable("SMTP_SERVER");
        string? SMTP_PORT = Environment.GetEnvironmentVariable("SMTP_PORT");
        string? SMTP_PASSWORD = Environment.GetEnvironmentVariable("SMTP_PASSWORD");

        public EmailService(IFileService fileService)
        {
            _fileService = fileService;
            EnvLoader.LoadEnvFile(".env");
            
        }

        public async Task SendAsync(EmailMember emailMember)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(SMTP_FROM_ADDRESS));
            email.To.Add(MailboxAddress.Parse(emailMember.to));
            email.Subject = email.Subject;

            try
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(
                    SMTP_SERVER,
                    int.Parse(SMTP_PORT),
                    SecureSocketOptions.StartTls
                );

                await smtp.AuthenticateAsync(
                    SMTP_FROM_ADDRESS,
                    SMTP_PASSWORD
                );


                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string templatePath = Path.Combine(basePath, "templates", "email_template.html");

                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = EmailBodyHelper.PrepareBody(templatePath, emailMember.subject, emailMember.link, emailMember.OTP)
                };

                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
    }
}
