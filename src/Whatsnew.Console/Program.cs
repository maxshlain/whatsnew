// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Whatsnew.Console;

var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true);

var configuration = builder.Build();
var userName = configuration.GetSection("GitHub").GetSection("UserName").Value
    ?? throw new NullReferenceException();
var pat = configuration.GetSection("GitHub").GetSection("Pat").Value
    ?? throw new NullReferenceException();


var issue = new MonitoredItem(
    Url: "https://github.com/maxshlain/whatsnew/issues/1",
    Type: "Github.Issue",
    Status: ""
);

var provider = new GithubInfoProvider(new GithubProxy(userName, pat));

var (changed, item) = await provider.TryGetInfoAsync(issue);

Console.WriteLine($"Changed: {changed}");
