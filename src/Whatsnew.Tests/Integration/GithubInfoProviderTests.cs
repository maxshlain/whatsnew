using FluentAssertions;
using Moq;
using Whatsnew.Console;
using Xunit.Abstractions;

namespace Whatsnew.Tests.Integration;

public class GithubInfoProviderTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public GithubInfoProviderTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TryGetInfoAsync_Returns_False_IfNotChanged()
    {
        var itemFromTest = new MonitoredItem(
            Url: "https://api.github.com/repos/maxshlain/whatsnew/issues/1",
            Type: "github",
            Status: "open"
        );

        var proxy = Mock.Of<IGithubProxy>(
            p => p.GetIssueData(It.IsAny<string>()) == Task.FromResult(itemFromTest)
        );

        var provider = new GithubInfoProvider(proxy);

        var item = new MonitoredItem
        (
            Url: "https://api.github.com/repos/maxshlain/whatsnew/issues/1",
            Type: "github",
            Status: "open"
        );

        var (changed, result) = await provider.TryGetInfoAsync(item);
        _testOutputHelper.WriteLine(result.ToString());
        changed.Should().BeFalse();
    }

    [Fact]
    public async Task TryGetInfoAsync_Returns_True_IfChanged()
    {
        var itemFromTest = new MonitoredItem(
            Url: "https://api.github.com/repos/maxshlain/whatsnew/issues/1",
            Type: "github",
            Status: "new"
        );

        var proxy = Mock.Of<IGithubProxy>(
            p => p.GetIssueData(It.IsAny<string>()) == Task.FromResult(itemFromTest)
        );

        var provider = new GithubInfoProvider(proxy);

        var item = new MonitoredItem
        (
            Url: "https://api.github.com/repos/maxshlain/whatsnew/issues/1",
            Type: "github",
            Status: "open"
        );

        var (changed, result) = await provider.TryGetInfoAsync(item);
        _testOutputHelper.WriteLine(result.ToString());
        changed.Should().BeTrue();
    }
}
