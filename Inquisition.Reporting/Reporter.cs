using Inquisition.Reporting.Handlers;
using Inquisition.Reporting.Models;

using System;
using System.IO;
using System.Threading.Tasks;

namespace Inquisition.Reporting
{
    public class Reporter
    {
        public EmailService EmailService { get => _emailService; set => _emailService = value; }
        public string OutputPath { get => _outputPath; set => _outputPath = value; }
        public string FileName { get => _fileName; set => _fileName = value; }
        public bool SendEmail { get => _sendEmail; set => _sendEmail = value; }

        private EmailService _emailService;
        private string _outputPath;
        private string _fileName;
        private bool _sendEmail;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Reporter()
        {
            _outputPath = Path.Combine("Reporter");
            _fileName = String.Format("{0:HH-mm-ss}.xml", DateTime.Now);
            _sendEmail = false;
        }

        /// <summary>
        /// Generates a log file based on a model implementing the IReport interface with
        /// the option to also send it via email.
        /// </summary>
        /// <typeparam name="T">Implementation of IReport interface</typeparam>
        /// <param name="report">Model implementing IReport interface</param>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task ReportAsync<T>(T report) where T : class, IReport
        {
            // Ensure folder is created
            Directory.CreateDirectory(_outputPath);

            // Add complete path to report
            report.Path = Path.Combine(_outputPath, _fileName);

            // Write to file
            XmlHandler.Serialize(report);

            if (!_sendEmail)
                return;

            // Send email with report
            await _emailService.SendEmailAsync(report);
        }
    }
}
