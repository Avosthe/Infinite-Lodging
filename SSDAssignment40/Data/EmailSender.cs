using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

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
            await Execute(Configuration["SendGrid:SendGridKey"], subject, message, email);
        }

        public async Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("accounts@infiniteLodging.org", "infiniteLodging");
            var to = new EmailAddress(email, "New User");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            var resp = await client.SendEmailAsync(msg);
            _logger.LogInformation("FIND ME");
            string x = await resp.Body.ReadAsStringAsync();
            _logger.LogInformation(x);
        }
    }
}
