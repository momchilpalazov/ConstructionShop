using DocumentFormat.OpenXml.Wordprocessing;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionShop.Uttility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email,subject,htmlMessage);
        }

       

        public async Task Execute(string email, string subject, string body)
        {

            MailjetClient client = new MailjetClient("ca91eddb8f2382430191ee792cd985dd", "0371bb1d9eac9f5d3ea6c997e4d9df11")
            {
                //Version = ApiVersion.V3_1,
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
        {"Email", "momchil.palazov@gmail.com"},
        {"Name", "Momchil"}
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
          "DotNet"
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
