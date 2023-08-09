using FluentAssertions;
using NumSharp;

namespace MCDA.NET.Tests;

[TestClass]
public class TopsisMethodTests
{
    [TestMethod]
    public void Resolve_Should_CalculatePreferenceValues()
    {
        // Arrange
        var expectedResult = new[] { 0.500, 0.617, 0.500 };

        var matrix = new NDArray(new double[,]
        {
            { 1, 2, 5 },
            { 3000, 3750, 4500 }
        }).T;

        var weights = new NDArray(new double[] { 0.5, 0.5 });
        var types = new NDArray(new double[] { -1, 1 });


        var topsis = new TopsisMethod(matrix, weights, types, NormalizationFunctions.MinMax);
        
        // Act
        var values = topsis.Resolve();
        var result = values.ToArray<double>().Select(x => Math.Round(x, 3));

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}