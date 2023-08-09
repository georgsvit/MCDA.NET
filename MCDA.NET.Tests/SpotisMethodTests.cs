using FluentAssertions;
using NumSharp;

namespace MCDA.NET.Tests;

[TestClass]
public class SpotisMethodTests
{
    [TestMethod]
    public void Resolve_Should_CalculatePreferenceValues()
    {
        // Arrange
        var expectedResult = new[] { 0.1989, 0.3705, 0.3063, 0.7491 };

        var matrix = new NDArray(new double[,]
        {
            { 10.5, -3.1, 1.7 },
            { -4.7, 0, 3.4 },
            { 8.1, 0.3, 1.3 },
            { 3.2, 7.3, -5.3 }
        });

        var bounds = new NDArray(new double[,]
        {
            { -5, 12 },
            { -6, 10 },
            { -8, 5 }
        });

        var weights = new NDArray(new double[] { 0.2, 0.3, 0.5 });
        var types = new NDArray(new double[] { 1, -1, 1 });

        var spotis = new SpotisMethod(matrix, weights, types, bounds);

        // Act
        var values = spotis.Resolve();
        var result = values.ToArray<double>().Select(x => Math.Round(x, 4));

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}