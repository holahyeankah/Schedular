using Microsoft.Extensions.Options;
using Schedular.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailSettings = Schedular.Models.MailSettings;

namespace Schedular.Interface
{
    public class MaillingService : IMaillingService
    {
        private readonly MailSettings _mailSettings;
        public MaillingService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task<Response> SendMail(GridEmailRequest request)
        {
            var apiKey = _mailSettings.Key;
            var client = new SendGridClient(apiKey);

            var sendGridMessage = new SendGridMessage();
            sendGridMessage.SetFrom(_mailSettings.email, "sjx-Logistics");
            sendGridMessage.AddTo(request.Email);
            sendGridMessage.SetTemplateId(request.templateIds);
            sendGridMessage.SetTemplateData(request);
            var response = await client.SendEmailAsync(sendGridMessage);
            return response;

        }
    }
}
