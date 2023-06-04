using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Common.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailManager : IEmailService
    {
        private readonly string _wwwrootPath;
        public EmailManager(string wwwrootPath)
        {
            _wwwrootPath = wwwrootPath;
        }
        public void SendEmailInformation(SendEmailInformationDto sendEmailInformationDto)
        {
            var htmlContent = File.ReadAllText($"{_wwwrootPath}\\email_templates\\email_information.html");

            htmlContent = htmlContent.Replace("{{subject}}", MessagesHelper.Email.Information.Subject);

            htmlContent = htmlContent.Replace("{{name}}", MessagesHelper.Email.Information.Name(sendEmailInformationDto.Name));

            htmlContent = htmlContent.Replace("{{informationMessage}}", MessagesHelper.Email.Information.InformationMessage);

            htmlContent = htmlContent.Replace("{{buttonText}}", MessagesHelper.Email.Information.ButtonText);

            htmlContent = htmlContent.Replace("{{buttonLink}}", MessagesHelper.Email.Information.ButtonLink);

            Send(new SendEmailDto(sendEmailInformationDto.Email,htmlContent, MessagesHelper.Email.Information.Subject));
        }

        private void Send(SendEmailDto sendEmailDto)
        {
            MailMessage message = new MailMessage();

            sendEmailDto.EmailAddresses.ForEach(emailAddress => message.To.Add(emailAddress));

            message.From = new MailAddress("noreply@entegraturk.com");

            message.Subject = sendEmailDto.Subject;

            message.IsBodyHtml = true;

            message.Body = sendEmailDto.Content;

            SmtpClient client = new SmtpClient();

            client.Port = 587;

            client.Host = "mail.entegraturk.com";

            client.EnableSsl = false;

            client.UseDefaultCredentials = false;

            client.Credentials = new NetworkCredential("noreply@entegraturk.com", "xzx2xg4Jttrbzm5nIJ2kj1pE4l");

            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            client.Send(message);

        }
    }
}
