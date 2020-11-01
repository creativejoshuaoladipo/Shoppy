using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shoppy.Utility
{
    public class EmailSender : IEmailSender
    {
        //Using the IConfiguration to get the Session of Appsetting.jon
        private readonly IConfiguration _configuration;
        private MailJetSetting mailjetSession { set; get; }

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            //Mapping the Appsetting.json session with the Class created(MailJetSetting)  to keep it's Property
            mailjetSession =  _configuration.GetSection("MailJetSetting").Get<MailJetSetting>();
            MailjetClient client = new MailjetClient(mailjetSession.ApiKey, mailjetSession.SecretKey)
            {
                Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "joshuaoladipo@protonmail.com"},
        {"Name", "Joshua"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          email
         }, {
          "Name",
          "MideAduBeautyShop"
         }
        }
       }
      }, {
       "Subject",
       subject
      }, {
       "HTMLPart",
       body
      }
     }
             });
            await client.PostAsync(request);
        }
    }
}
