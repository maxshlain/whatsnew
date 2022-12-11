using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Whatsnew.Console;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();
            
        using IHost host = Host
        .CreateDefaultBuilder(args)
        .ConfigureServices(ConfigureService)
        .UseSerilog((context, services, loggerConfiguration) => 
            loggerConfiguration
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
        )
        .Build();

        Log.Logger.Information("Starting host");
        await host.RunAsync();
    }

    private static void ConfigureService(HostBuilderContext context, IServiceCollection services)
    {

        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true);

        var configuration = builder.Build();
        var userName = configuration.GetSection("GitHub").GetSection("UserName").Value
            ?? throw new NullReferenceException();
        var pat = configuration.GetSection("GitHub").GetSection("Pat").Value
            ?? throw new NullReferenceException();

        var proxySettings = new GithubProxySettings(userName, pat);

        services.AddSingleton(proxySettings);
        services.AddHostedService<WhatsNewHostedService>();
        services.AddSingleton<IGithubProxy, GithubProxy>();
        services.AddSingleton<GithubInfoProvider>();
    }
}
