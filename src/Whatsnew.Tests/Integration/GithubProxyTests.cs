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
        var proxy = new GithubProxy("");
        
        proxy.Should().NotBeNull();
    }

    [Fact]
    public async Task GetDoesNotThrow()
    {
        var pat = new AppSettingsAccessor()
            .Build()
            .GetSection("Github")
            .GetSection("Pat")
            .Value;
        
        var proxy = new GithubProxy(pat ?? throw new Exception("Pat not found"));

        var result = await proxy.GetIssueData("");
        _testOutputHelper.WriteLine(result);
        result.Should().NotBeNull();
    }
}