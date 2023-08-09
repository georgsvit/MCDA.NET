using FluentAssertions;
using NumSharp;

namespace MCDA.NET.Tests;

[TestClass]
public class VikorMethodTests
{
    [TestMethod]
    public void Resolve_Should_CalculatePreferenceValues()
    {
        // Arrange
        var expectedResult = new[] { 0.5679, 0.7667, 1, 0.7493, 0 };

        var matrix = new NDArray(new double[,]
        {
            { 78, 56, 34, 6 },
            { 4, 45, 3, 97 },
            { 18, 2, 50, 63 },
            { 9, 14, 11, 92 },
            { 85, 9, 100, 29 }
        });

        var weights = new NDArray(new double[] { 0.25, 0.25, 0.25, 0.25 });
        var types = new NDArray(new double[] { 1, 1, 1, 1 });

        var vikor = new VikorMethod(matrix, weights, types, (m, cost) => m);

        // Act
        var values = vikor.Resolve();
        var result = values.ToArray<double>().Select(x => Math.Round(x, 4));

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}
