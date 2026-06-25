using Xunit;
using FluentAssertions;

namespace CoffeeShop.Tests;

public class OrderServiceTests
{
    [Fact]
    public void Test_KiemTraPhepToanCoBan_ShouldReturnTrue()
    {
        int a = 5;
        int b = 5;
        int result = a + b;
        result.Should().Be(10);
    }
}
