using Inquisition.Reporting.Extensions;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.IO;

using Xunit;

namespace Inquisition.Reporting.Tests
{
    public class ReporterBuilderTests
    {
        [Fact]
        public void AppendXmlExtensionToFile()
        {
            // Create file with no extension
            Reporter reporter = new ReporterBuilder()
                .SetFileName("testFilename")
                .Build();

            // Check if RepoterBuilder added extension
            Assert.EndsWith(".xml", reporter.FileName);
        }

        [Fact]
        public void CanPassEmailConfigWithActionDelegate()
        {
            // Configure email using Action<T> delegate
            Reporter reporter = new ReporterBuilder()
                .SetEmailService(config => {
                    config.EmailProvider = EmailProvider.Manual;
                    config.Host = "test";
                    config.Port = 123;
                    config.Username = "Heisenberg";
                    config.Password = "SuperSecretPassword";
                    config.FromAddress = "someone@someemail.com";
                    config.ToAddress = "someoneelse@email.com";
                })
                .Build();
            
            // Check config saved
            Assert.Equal("test", reporter.EmailService.Host);
        }

        [Fact]
        public void CanRegisterIntoServiceCollection()
        {
            Reporter reporter = null;

            // Listen for exception
            Exception exception = Record.Exception(() => {
                // Use extension to add to ServiceCollection
                IServiceProvider services = new ServiceCollection()
                    .AddReporter()
                    .BuildServiceProvider();

                // Call back from ServiceCollection
                 reporter = services.GetService<Reporter>();
            });

            // No exception thrown
            Assert.Null(exception);

            // Reporter registered into Servicecollection and called back out
            Assert.NotNull(reporter);
        }

        [Fact]
        public void ReporterHasDefaultRepoterConfig()
        {
            // Default repoter
            Reporter reporter = new Reporter();

            // Reporter was configured with the default RepoterConfig parameters
            Assert.Equal(Path.Combine("Reporter"), reporter.OutputPath);
            Assert.False(reporter.SendEmail);
        }

        [Fact]
        public void RepoterHasDefaultConfigFromServiceCollection()
        {
            // Use extension to add to ServiceCollection
            IServiceProvider services = new ServiceCollection()
                    .AddReporter()
                    .BuildServiceProvider();

            // Call back from ServiceCollection
            Reporter reporter = services.GetService<Reporter>();

            // Reporter was configured with the default RepoterConfig parameters
            Assert.Equal(Path.Combine("Reporter"), reporter.OutputPath);
            Assert.False(reporter.SendEmail);
        }
    }
}
