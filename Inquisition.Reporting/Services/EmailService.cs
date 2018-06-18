using Inquisition.Reporting.Handlers;
using Inquisition.Reporting.Models;
using Inquisition.Reporting.Properties;

using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Inquisition.Reporting
{
    /// <summary>
    /// Used to send email containing IReport file
    /// </summary>
    public class EmailService
    {
        public EmailProvider EmailProvider { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string XSLFile { get; set; }

        /// <summary>
        /// Make sure Host and Port are set.
        /// </summary>
        private void CheckEmailProvider()
        {
            switch (EmailProvider)
            {
                case EmailProvider.Google:
                    Host = ResourcesGoogle.Host;
                    Port = int.Parse(ResourcesGoogle.Port);
                    break;
                case EmailProvider.Microsoft:
                    Host = ResourcesHotmail.Host;
                    Port = int.Parse(ResourcesHotmail.Port);
                    break;
                case EmailProvider.Manual:
                    if (Host is null)
                        throw new ArgumentNullException(nameof(Host));

                    if (Port is 0)
                        throw new ArgumentException(nameof(Port));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Send email with an error report
        /// </summary>
        /// <param name="report">Implementation of IReport interface</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task SendEmailAsync<T>(T report) where T : class, IReport
        {
            CheckEmailProvider();

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
                email.IsBodyHtml = false;
                email.Body = "An error report was generated, see attached file";
            }

            email.Attachments.Add(new Attachment(report.Path));

            await client.SendMailAsync(email);
        }
    }
}
