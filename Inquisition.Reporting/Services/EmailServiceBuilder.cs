
using System;
using System.IO;

namespace Inquisition.Reporting
{
    public class EmailServiceBuilder
    {
        private EmailService _emailService;

        /// <summary>
        /// Default constructor
        /// </summary>
        public EmailServiceBuilder()
        {
            _emailService = new EmailService();
        }

        /// <summary>
        /// EmailProvider.Google and EmailProvider.Mictosoft have pre-defined hostname
        /// and ports, in case you want to use your own, use EmailProvider.Manual.
        /// </summary>
        /// <param name="emailProvider">Call SetHostAndPort() method if EmailProvider is Manual</param>
        /// <returns></returns>
        public EmailServiceBuilder SetEmailProvider(EmailProvider emailProvider)
        {
            _emailService.EmailProvider = emailProvider;

            return this;
        }

        /// <summary>
        /// Specify the username and password for the Email service.
        /// </summary>
        /// <param name="username">Email service username used to login</param>
        /// <param name="password">Email service password used to login</param>
        /// <returns></returns>
        public EmailServiceBuilder SetCredentials(string username, string password)
        {
            if (username is null)
                throw new ArgumentNullException(nameof(username));

            if (password is null)
                throw new ArgumentNullException(nameof(password));

            _emailService.Username = username;
            _emailService.Password = password;

            return this;
        }

        /// <summary>
        /// Specify the sender and reciever email addresses
        /// </summary>
        /// <param name="sender">Who sends the email</param>
        /// <param name="reciever">Who to send the email to</param>
        /// <returns></returns>
        public EmailServiceBuilder SetEmailAddresses(string sender, string reciever)
        {
            if (sender is null)
                throw new ArgumentNullException(nameof(sender));

            if (reciever is null)
                throw new ArgumentNullException(nameof(reciever));

            _emailService.FromAddress = sender;
            _emailService.ToAddress = reciever;

            return this;
        }

        /// <summary>
        /// Only use this method if in .SetEmailProvider() you specified EmailProvider.Manual
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public EmailServiceBuilder SetHostAndPort(string host, int port)
        {
            if (host is null)
                throw new ArgumentNullException(nameof(host));

            if (port is 0)
                throw new ArgumentException(nameof(port));

            _emailService.Host = host;
            _emailService.Port = port;

            return this;
        }

        /// <summary>
        /// Set a XSLT file to transform the XML file into a HTML Email body
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <returns></returns>
        public EmailServiceBuilder SetXslFile(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            // Check for file
            if (!File.Exists(path))
                throw new FileNotFoundException(nameof(path));

            _emailService.XSLFile = path;

            return this;
        }

        /// <summary>
        /// Build a EmailService instance
        /// </summary>
        /// <returns>A EmailService instance</returns>
        public EmailService Build()
        {
            return _emailService;
        }
    }
}
