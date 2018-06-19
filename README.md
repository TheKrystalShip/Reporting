![NuGet](https://img.shields.io/nuget/dt/Inquisition.Reporting.svg) ![NuGet](https://img.shields.io/nuget/v/Inquisition.Reporting.svg)

[NuGet package](https://www.nuget.org/packages/Inquisition.Reporting/)

# Inquisition.Reporting

Library to create XML files from Exceptions with the posibility to also send the report via Email.

## Dependency Injection

This library offers an extension method for IServiceCollection under `Inquisition.Reporting.Extensions` in case you want to use this with Dependency Injection:

```csharp
public static IServiceCollection AddReporter(this IServiceCollection services)
{
    services.AddSingleton<ReporterBuilder>();
    services.AddSingleton<Reporter>();

    return services;
}
```

> Note that this extension method does **NOT** include the `EmailService` configuration inside of the `Reporter` instance, since the `EmailService` needs to be manually configured. This instance only creates the reports locally.

## Setup

First thing you need is an implementation of the `IReport` interface found inside the `Inquisition.Reporting.Models` namespace.

This library offers a default implementation under that same namespace. You can use the default `Report` implementation or make your own that implements the `IReport` interface.

`IReport` interface:

```csharp
public interface IReport
{
    Guid Guid { get; set; }
    string ErrorMessage { get; set; }
    string StackTrace { get; set; }
    string Path { get; set; }
}
```

### Reporter

The `Reporter` class is responsible for generating the local report files and also sending them via email.

In order to use the `Reporter`, an instance of it needs to be configured using the `ReporterBuilder` class. A default instance can be created like this:

```csharp
Reporter reporter = new RepotertBuilder().Build();
```

In this case the `Reporter` will use the default configuration for the files it creates:

```csharp
_outputPath = Path.Combine("Reporter");
_fileName = String.Format("{0:HH-mm-ss}.xml", DateTime.Now);
_sendEmail = false;
```

### EmailService

> Note: The `EmailService` class can be used on it's own without the need for the `Reporter` class.

One of the options the `ReporterBuilder` has is `.SetEmailService()`, which accepts either an `Action<EmailConfig>` delegate or an instance of `EmailService`, which can be created by using the `EmailServiceBuilder` class, in a similar way to the `ReporterBuilder` but with a few diferences.

Since `EmailService` needs some parameters that cannot have a default (Like a account Username and Password for example), the `EmailServiceBuilder` does not offer a default instance.

But instead a full instance must be configured in order for it to work, here's an example:

```csharp
public EmailService CreateEmailService()
{
    EmailService emailService = new EmailServiceBuilder()
        .SetEmailProvider(EmailProvider.Google)
        .SetCredentials("Username", "Password")
        .SetEmailAddresses("from@gmail.com", "to@gmail.com")
        .Build();

    return emailService;
}
```

In case you want to use your own email provider, you will have to configure the hostname and port yourself, but there's a method that helps with that:

```csharp
EmailService emailService = new EmailServiceBuilder()
    .SetEmailProvider(EmailProvider.Manual)
    .SetHostAndPort("smtp.myownemail.com", 9000)
    .Build();
```

> Note: The `.SetHostAndPort()` method overrides even the predefined `EmailProvider.Google` and `EmailProvider.Microsoft` configuration, but is not needed if one of the two `EmailProvider` enums is specified. It **IS** needed however when using `EmailProvider.Manual` or else the `EmailService` will throw an exception because they cannot have default values, also if for some reason the `EmailService` fails to connect with the predefined hostname and port, you can override them with whatever hostname and port you want.

Other options include the possibility to add a XSLT transformation file in order to transform the XML report file into a HTML file, which will be inserted into the email body.

```csharp
EmailService emailService = new EmailServiceBuilder()
    .SetXslFile("pathToFile")
    .Build();
```

## Ok, so how do I use this?

Alright, here's some actually useful code on how you can use this library:

### Setup

#### For the default instance:

In ASP.Net using Dependency Injection:

`Startup.cs`
Using the extension method that adds the default instance to the IoC container.
```csharp
using Inquisition.Reporting.Extensions
// ...

public void ConfigureServices(IServiceCollection services)
{
    services.AddLogger();
    //...
}

```

#### Custom instance

This instance has a `EmailService` configured and will use it to also send the reports via email.

```csharp
using Inquisition.Reporting
//...

public void ConfigureServices(IServiceCollection services)
{
    EmailService emailService = new EmailServiceBuilder()
        .SetEmailProvider(EmailProvider.Google)
        .SetCredentials("Username", "Password")
        .SetEmailAddresses("from@gmail.com", "to@gmail.com")
        .SetXslFile("transform.xsl")
        .Build();

    Reporter reporter = new ReporterBuilder()
        .SetFileName("filename")
        .SetOutputPath("outputPath")
        .SetEmailService(emailService)
        .Build();

    services.AddSingleton(reporter);
    //...
}
```

An instance without the `EmailService` would look like this:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    Reporter reporter = new ReporterBuilder()
        .SetFileName("filename")
        .SetOutputPath("outputPath")
        .Build();

    services.AddSingleton(reporter);
    //...
}
```

### Usage

Inside a controller:

```csharp
using Inquisition.Reporting
//...

public class ValuesController
{
    private readonly Reporter _reporter;

    // Inject into constructor
    public ValuesController(Reporter reporter)
    {
        _reporter = reporter;
    }
    // ...

    [HttpGet]
    public async Task<ActionResult> GetValues()
    {
        try
        {
            // Some database access with risk of throwing
        }
        catch (Exception e)
        {
            await _reporter.ReportAsync(new Report(e));
        }
    }
}
```


