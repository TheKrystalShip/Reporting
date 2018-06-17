using System;
using System.IO;

namespace Inquisition.Reporting
{
    public class ReporterConfig
    {
        /// <summary>
        /// Directory to write file to.
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// Name of the file that will be written to disk
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Bool value to send email after generating error log file. If set to true, must specify email
        /// service details.
        /// </summary>
        public bool SendEmail { get; set; }

        /// <summary>
        /// Email service host url (Example: smtp.gmail.com).
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// SMTP Client port (Example: 587 for Gmail).
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Email account username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Email account password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Email address from which email is sent (Usually same as Username).
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// Email address to send email to.
        /// </summary>
        public string ToAddress { get; set; }

        /// <summary>
        /// XSLT file to transform IReport model into a valid Email body html (Only necessary if SendEmail = true).
        /// </summary>
        public string XSLFile { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReporterConfig()
        {
            OutputPath = Path.Combine("Reporter", "Logs");
            FileName = String.Format("{0:HH-mm-ss}.xml", DateTime.Now);
            SendEmail = false;
        }
    }
}
