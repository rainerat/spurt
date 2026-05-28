using FluentAssertions;
using Spurt.App.Services;
using Xunit;

namespace Spurt.App.Tests.Services;

public class ThemeServiceTests
{
    [Theory]
    [InlineData("System")]
    [InlineData("Light")]
    [InlineData("Dark")]
    public void Normalize_ValidModes_ArePreserved(string mode)
    {
        var service = new ThemeService();

        service.Normalize(mode).Should().Be(mode);
    }

    [Fact]
    public void Normalize_InvalidMode_FallsBackToSystem()
    {
        var service = new ThemeService();

        service.Normalize("Neon").Should().Be("System");
    }
}
