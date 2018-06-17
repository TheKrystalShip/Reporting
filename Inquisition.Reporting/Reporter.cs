using Inquisition.Reporting.Handlers;
using Inquisition.Reporting.Models;
using Inquisition.Reporting.Services;

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Inquisition.Reporting
{
    public class Reporter
    {
        private readonly ReporterConfig _config;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="config">
        /// ReporterConfig to specify some paramenters on how to make reports
        /// </param>
        public Reporter(ReporterConfig config)
        {
            _config = config;
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
            Directory.CreateDirectory(_config.OutputPath);
            report.Path = Path.Combine(_config.OutputPath, _config.FileName);

            XmlHandler.Serialize(report);

            if (!_config.SendEmail)
                return;

            foreach (PropertyInfo property in _config.GetType().GetProperties())
            {
                if (property.GetValue(_config) is null)
                    throw new ArgumentNullException($"{property.Name} not specified");
            }

            EmailService emailService = new EmailService()
            {
                Host = _config.Host,
                Port = _config.Port,
                Username = _config.Username,
                Password = _config.Password,
                FromAddress = _config.FromAddress,
                ToAddress = _config.ToAddress,
                XSLFile = _config.XSLFile
            };

            await emailService.SendEmailAsync(report);
        }
    }
}
