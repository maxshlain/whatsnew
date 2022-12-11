using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Whatsnew.Console;

public static class AppConfigurator
{
    public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
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
