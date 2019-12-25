using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AvitoHelper.Services
{
    public class EmailSender : IEmailSender
    {
        ILogger<EmailSender> _loger { get; }
        public EmailSender(ILogger<EmailSender> loger)
        {
            this._loger = loger;
        }
        public void Execute(string login, string pass, string to, string subject, string text)
        {
            try
            {
                _loger.LogDebug("Try SendEmail");   
                using (MailMessage message = new MailMessage(login, to))
                {
                    message.Subject = subject;
                    message.Body = text;
                    message.IsBodyHtml = true;
                    SmtpClient client = new SmtpClient
                    {
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = true,
                        EnableSsl = true,
                        Host = Environment.GetEnvironmentVariable("EMAIL_SMTP_HOST"),
                        Port = int.Parse(Environment.GetEnvironmentVariable("EMAIL_SMTP_PORT")),
                        Credentials = new NetworkCredential(login, pass),
                    };
                    client.Send(message);
                    _loger.LogDebug("Success Email");
                }
            }
            catch (Exception ex)
            {
                _loger.LogError("Custom Error = " + ex.ToString());
            }
        }
    }
}
