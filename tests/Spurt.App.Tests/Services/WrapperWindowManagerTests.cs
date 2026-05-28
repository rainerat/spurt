using FluentAssertions;
using Spurt.App.Services;
using Xunit;

namespace Spurt.App.Tests.Services;

public class WrapperWindowManagerTests
{
    [Fact]
    public void Navigate_CreatesWrapperOnceAndReuses()
    {
        var manager = new WrapperWindowManager();
        manager.Navigate("https://example.com/one");
        manager.Navigate("https://example.com/two");

        manager.CreatedCount.Should().Be(1);
        manager.LastUri.Should().Be("https://example.com/two");
    }

    [Fact]
    public void Navigate_AfterClose_CreatesAgain()
    {
        var manager = new WrapperWindowManager();
        manager.Navigate("https://example.com/one");
        manager.Close();
        manager.Navigate("https://example.com/two");

        manager.CreatedCount.Should().Be(2);
    }
}
