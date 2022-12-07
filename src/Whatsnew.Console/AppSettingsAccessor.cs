using Microsoft.Extensions.Configuration;

namespace Whatsnew.Console;

public class AppSettingsAccessor
{
    private IConfigurationRoot? _configuration;

    public AppSettingsAccessor Build()
    {
         _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
            .Build();

         return this;
    }
    
    public IConfigurationSection GetSection(string key)
    {
        return _configuration?.GetSection(key) ?? throw new Exception("Not initialized");
    }
}