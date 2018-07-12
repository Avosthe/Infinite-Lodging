using MessageBird;
using MessageBird.Exceptions;
using MessageBird.Objects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    public class SmsSender : ISmsSender
    {

        const string AccessKey = "ZwdyutkaI90t2VcEBdS0jBO2X";
        //private class Message
        //{
        //    public string recipients { get; set; }
        //    public string originator { get; set; }
        //    public string body { get; set; }
        //}

        public ILogger _logger { get; set; }
            public SmsSender(ILogger<SmsSender> logger)
        {
            _logger = logger;
        }

        public bool SendSms(string number, string message)
        {
            Client client = Client.CreateDefault(AccessKey);
            long recipient = Convert.ToInt64(number);
            try
            {
                MessageBird.Objects.Message Response = client.SendMessage("Inf Lodging", message, new[] { recipient });
            }
            catch(ErrorException e)
            {
                if (e.HasErrors)
                {
                    foreach (Error error in e.Errors)
                    {
                        _logger.LogInformation($"{error.Code}, {error.Description}, {error.Parameter}");
                        // error.Code, error.Description, error.Parameter
                    }
                }
                return false;
            }

            return true;
            //using (var client = new HttpClient())
            //{

            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("AccessKey", "ZwdyutkaI90t2VcEBdS0jBO2X");
            //    client.BaseAddress = new Uri("https://rest.messagebird.com/messages");
            //    //serialize your json using newtonsoft json serializer then add it to the StringContent
            //    Message payload = new Message()
            //    {
            //        recipients = number,
            //        originator = "Infinite Lodging",
            //        body = message
            //    };

            //    var JsonContent = JsonConvert.SerializeObject(payload);

            //    var content = new StringContent(JsonContent, Encoding.UTF8, "application/json");

            //    var result = await client.PostAsync("https://rest.messagebird.com/messages", content);
            //    string resultContent = await result.Content.ReadAsStringAsync();

            //}
        }
    }
}
