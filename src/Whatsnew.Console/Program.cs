using Microsoft.Extensions.Hosting;
using Serilog;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            await BuildHostAndRunAsync(args);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static async Task BuildHostAndRunAsync(string[] args)
    {
        using IHost host = Host
                    .CreateDefaultBuilder(args)
                    .ConfigureServices(AppConfigurator.ConfigureServices)
                    .UseSerilog()
                    .Build();

        Log.Information("Host is starting");
        await host.RunAsync();
        Log.Information("Host stopped");
    }
}
