using Microsoft.Extensions.Options;
using Schedular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Schedular.Interface
{
    public class MaillingService : IMaillingService
    {
        private readonly MailSettings _mailSettings;
        public MaillingService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

       public void SendMail(string body, string email)
        {
            MailMessage message = new MailMessage(_mailSettings.Mail, email);
            message.Subject = $"Welcome to{_mailSettings.DisplayName}";
            message.IsBodyHtml = true;
            message.Body = body;
            message.BodyEncoding = Encoding.UTF8;
            try
            {
                SmtpClient smtp = new SmtpClient(_mailSettings.Host, _mailSettings.Port);
                smtp.Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Send(message);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
