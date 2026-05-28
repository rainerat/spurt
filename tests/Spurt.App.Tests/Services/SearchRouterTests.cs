using System;
using FluentAssertions;
using Spurt.App.Services;
using Xunit;

namespace Spurt.App.Tests.Services;

public class SearchRouterTests
{
    [Fact]
    public void BuildSearchUri_EncodesQuery()
    {
        var router = new SearchRouter();

        var uri = router.BuildSearchUri("https://www.google.com/search?q={query}", "c# tuples");

        uri.Should().Be("https://www.google.com/search?q=c%23%20tuples");
    }

    [Fact]
    public void BuildSearchUri_ThrowsOnEmptyQuery()
    {
        var router = new SearchRouter();
        Action act = () => router.BuildSearchUri("https://www.google.com/search?q={query}", " ");

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void BuildSearchUri_ThrowsOnInvalidTemplate()
    {
        var router = new SearchRouter();
        Action act = () => router.BuildSearchUri("https://www.google.com/search", "test");

        act.Should().Throw<ArgumentException>();
    }
}
