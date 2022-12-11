using FluentAssertions;
using Whatsnew.Console;
using Xunit.Abstractions;

namespace Whatsnew.Tests.Integration;

public class GithubProxyTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public GithubProxyTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Constructor_DoesNotThrow()
    {
        var proxy = new GithubProxy(new GithubProxySettings("user", "pat"));
        
        proxy.Should().NotBeNull();
    }

    [Fact]
    public async Task GetDoesNotThrow()
    {
        var settings = new AppSettingsAccessor()
            .Build()
            .GetSection("Github");

        var userName = settings 
            .GetSection("UserName")
            .Value ?? throw new Exception("UserName not found");

        var pat = settings 
            .GetSection("Pat")
            .Value ?? throw new Exception("Pat not found");
        
        var proxy = new GithubProxy(new GithubProxySettings(userName, pat));

        var result = await proxy.GetIssueData("https://api.github.com/repos/maxshlain/whatsnew/issues/1");
        _testOutputHelper.WriteLine(result.ToString());
        result.Should().NotBeNull();
    }
}
