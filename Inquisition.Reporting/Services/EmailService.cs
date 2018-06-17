using Inquisition.Reporting.Handlers;
using Inquisition.Reporting.Models;

using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Inquisition.Reporting.Services
{
    internal class EmailService
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string XSLFile { get; set; }

        /// <summary>
        /// Send email with an error report
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task SendEmailAsync<T>(T report) where T : class, IReport
        {
            try
            {
                SmtpClient client = new SmtpClient
                {
                    Host = Host,
                    Port = Port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(Username, Password)
                };

                MailAddress from = new MailAddress(FromAddress);
                MailAddress to = new MailAddress(ToAddress);

                MailMessage email = new MailMessage(from.Address, to.Address)
                {
                    Subject = $"{DateTime.Now} - Error Report"
                };

                if (XSLFile != null)
                {
                    email.IsBodyHtml = true;
                    email.Body = XmlHandler.Transform(report.Path, XSLFile);
                }
                else
                {
                    email.Attachments.Add(new Attachment(report.Path));
                }

                await client.SendMailAsync(email);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
