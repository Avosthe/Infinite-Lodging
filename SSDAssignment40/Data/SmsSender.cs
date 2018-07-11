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
        private class Message
        {
            public string recipients { get; set; }
            public string originator { get; set; }
            public string body { get; set; }
        }

        public async Task SendSmsAsync(string number, string message)
        {
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("AccessKey", "ZwdyutkaI90t2VcEBdS0jBO2X");
                client.BaseAddress = new Uri("https://rest.messagebird.com/messages");
                //serialize your json using newtonsoft json serializer then add it to the StringContent
                Message payload = new Message()
                {
                    recipients = number,
                    originator = "Infinite Lodging",
                    body = message
                };

                var JsonContent = JsonConvert.SerializeObject(payload);

                var content = new StringContent(JsonContent, Encoding.UTF8, "application/json");

                var result = await client.PostAsync("https://rest.messagebird.com/messages", content);
                string resultContent = await result.Content.ReadAsStringAsync();

            }
        }
    }
}
