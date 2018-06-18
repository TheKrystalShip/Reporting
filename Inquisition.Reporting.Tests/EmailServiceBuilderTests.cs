using Xunit;

namespace Inquisition.Reporting.Tests
{
    public class EmailServiceBuilderTests
    {
        private EmailService _service;

        public EmailServiceBuilderTests()
        {
            _service = new EmailService();
        }

        [Fact]
        public void CanBuildEmailService()
        {
            Assert.NotNull(_service);
        }

        [Fact]
        public void HostIsNullOnDefaultConstructor()
        {
            Assert.Null(_service.Host);
        }

        [Fact]
        public void PortIsZeroOnDefaultConstructor()
        {
            Assert.Equal(0, _service.Port);
        }

        public EmailService CreateEmailService()
        {
            EmailService emailService = new EmailServiceBuilder()
                .SetEmailProvider(EmailProvider.Google)
                .SetCredentials("Username", "Password")
                .SetEmailAddresses("from@gmail.com", "to@gmail.com")
                .SetXslFile("transform.xsl")
                .Build();

            return emailService;
        }
    }
}
