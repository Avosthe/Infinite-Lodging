using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;

namespace SSDAssignment40.Data
{
    public class EmailSender : IEmailSender
    {
        IConfiguration Configuration { get; set; }
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IConfiguration config, ILogger<EmailSender> logger)
        {
            Configuration = config;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {

            var client = new MailjetClient(Configuration["MailJet:apiKey"], Configuration["MailJet:apiSecret"]);
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.FromEmail, "accounts@evocreate.tk")
            .Property(Send.FromName, "Infinite Lodging")
            .Property(Send.Subject, subject)
            .Property(Send.TextPart, message)
            .Property(Send.HtmlPart, message)
            .Property(Send.Recipients, new JArray {
                new JObject {
                 {"Email", email}
                 }
                });
            MailjetResponse response = await client.PostAsync(request);
            _logger.LogInformation(response.IsSuccessStatusCode.ToString());
        }
    }
}
