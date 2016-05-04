using System;
using System.Collections.Generic;
using System.Net.Mail;
using Castle.Core.Internal;

namespace BSSiseveeb.Public.Web.Controllers.API.Helpers
{
    public class EmailHelper
    {
        public static void SendEmail(List<String> emails, String subject, String body)
        {
            var message = new MailMessage();
            foreach (var email in emails)
            {
                message.To.Add(new MailAddress(email));
            }
            if (!emails.IsNullOrEmpty())
            {
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    smtp.Send(message);
                }
            }
        }
    }
}