using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Whatsnew.Console;
using Xunit.Abstractions;

namespace Whatsnew.Tests;

public class GithubInfoProviderTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    private readonly MonitoredItem _openItem = new MonitoredItem(
          Url: "https://api.github.com/repos/maxshlain/whatsnew/issues/1",
          Type: "github",
          Status: "open"
      );

    private readonly MonitoredItem _newItem = new MonitoredItem(
        Url: "https://api.github.com/repos/maxshlain/whatsnew/issues/1",
        Type: "github",
        Status: "new"
    );

    public GithubInfoProviderTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private GithubInfoProvider BuildGithubInfoProvider(IGithubProxy proxy)
    {
        return new ServiceCollection()
           .AddLogging((builder) => builder.AddXUnit(_testOutputHelper))
           .AddSingleton(proxy)
           .AddSingleton<GithubInfoProvider>()
           .BuildServiceProvider()
           .GetRequiredService<GithubInfoProvider>();
    }

    [Fact]
    public async Task TryGetInfoAsync_Returns_False_IfNotChanged()
    {
        // ARRANGE
        var proxy = Mock.Of<IGithubProxy>(
            p => p.GetIssueData(It.IsAny<string>()) == Task.FromResult(_openItem)
        );

        var provider = BuildGithubInfoProvider(proxy);

        // ACT
        var (changed, result) = await provider.TryGetInfoAsync(_openItem);

        // ASSERT
        _testOutputHelper.WriteLine(result.ToString());
        changed.Should().BeFalse();
    }

    [Fact]
    public async Task TryGetInfoAsync_Returns_True_IfChanged()
    {
        // ARRANGE
        var proxy = Mock.Of<IGithubProxy>(
            p => p.GetIssueData(It.IsAny<string>()) == Task.FromResult(_openItem)
        );

        // ACT
        var provider = BuildGithubInfoProvider(proxy);

        // ASSERT
        var (changed, result) = await provider.TryGetInfoAsync(_newItem);
        _testOutputHelper.WriteLine(result.ToString());
        changed.Should().BeTrue();
    }
}
