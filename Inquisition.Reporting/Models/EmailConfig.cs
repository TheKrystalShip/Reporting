namespace Inquisition.Reporting
{
    public class EmailConfig
    {
        public EmailProvider EmailProvider { get; set; }

        /// <summary>
        /// Email server hostname
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Email server port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Account username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Account password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Address from which the email is sent from
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// Address to which the email is sent to
        /// </summary>
        public string ToAddress { get; set; }
    }
}
