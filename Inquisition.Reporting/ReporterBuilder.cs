using System;

namespace Inquisition.Reporting
{
    public class ReporterBuilder
    {
        private Reporter _reporter;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReporterBuilder()
        {
            _reporter = new Reporter();
        }

        /// <summary>
        /// Specify an output directory (Must already exist)
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public ReporterBuilder SetOutputPath(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            _reporter.OutputPath = path;
            return this;
        }

        /// <summary>
        /// Filename to be written to (.xml extension added automatically if not already present)
        /// </summary>
        /// <param name="filename"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public ReporterBuilder SetFileName(string filename)
        {
            if (filename is null)
                throw new ArgumentNullException(nameof(filename));

            // Append extension if not present
            if (!filename.EndsWith(".xml"))
                filename += ".xml";

            _reporter.FileName = filename;
            return this;
        }

        /// <summary>
        /// Configure the EmailService instance to be used by the Reporter.
        /// </summary>
        /// <param name="config"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public ReporterBuilder SetEmailService(Action<EmailConfig> config)
        {
            if (config is null)
                throw new ArgumentNullException();

            // Thanks Amy for the help with this:
            EmailConfig emailConfig = new EmailConfig();
            config(emailConfig);

            // Build an EmailService using the EmailConfig param
            EmailServiceBuilder emailServiceBuilder = new EmailServiceBuilder()
                .SetEmailProvider(emailConfig.EmailProvider)
                .SetCredentials(emailConfig.Username, emailConfig.Password)
                .SetEmailAddresses(emailConfig.FromAddress, emailConfig.ToAddress);

            // EmailProvider.Google and EmailProvider.Hotmail have already defined ports and hostname
            if (emailConfig.EmailProvider is EmailProvider.Manual)
                emailServiceBuilder.SetHostAndPort(emailConfig.Host, emailConfig.Port);
            
            _reporter.EmailService = emailServiceBuilder.Build();

            return this;
        }

        /// <summary>
        /// Configure the EmailService instance to be used by the Reporter.
        /// Use the EmailServiceBuilder class to make a EmailService instance.
        /// </summary>
        /// <param name="emailService"></param>
        /// <returns></returns>
        public ReporterBuilder SetEmailService(EmailService emailService)
        {
            if (emailService is null)
                throw new ArgumentNullException(nameof(emailService));

            _reporter.EmailService = emailService;

            return this;
        }

        public Reporter Build()
        {
            return _reporter;
        }
    }
}
