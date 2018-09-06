using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace SocialEngineeringReporting.Models
{
    public class SendEmailModel
    {
        public string SmtpServer { get; } = "webmail.cdc.gov.tw";
        public int SmtpPort { get; } = 25;
        public MailAddress From { get; set; }
        public List<string> To { get; set; }
        public string Subject { get; set; }
         
        public void Send(string body)
        {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = From;
                    foreach (var addr in To)
                    {
                        mail.To.Add(new MailAddress($"{addr}@cdc.gov.tw"));
                    }

                    mail.Subject = Subject;
                    mail.Body = body;
                    mail.IsBodyHtml = false;
                    using (SmtpClient smtp = new SmtpClient(SmtpServer, SmtpPort))
                    {
                        smtp.Send(mail);
                    }
                }
            }


        

        
    }
}


