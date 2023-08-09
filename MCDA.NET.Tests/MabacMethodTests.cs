using FluentAssertions;
using NumSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCDA.NET.Tests;

[TestClass]
public class MabacMethodTests
{
    [TestMethod]
    public void Resolve_Should_CalculatePreferenceValues()
    {
        // Arrange
        var expectedResult = new[] { 0.0826, 0.2183, -0.0488, 0.0246, -0.0704, 0.0465, 0.0464 };

        var matrix = new NDArray(new double[,]
        {
            { 22600, 3800, 2, 5, 1.06, 3.00, 3.5, 2.8, 24.5, 6.5 },
            { 19500, 4200, 3, 2, 0.95, 3.00, 3.4, 2.2, 24, 7.0 },
            { 21700, 4000, 1, 3, 1.25, 3.20, 3.3, 2.5, 24.5, 7.3 },
            { 20600, 3800, 2, 5, 1.05, 3.25, 3.2, 2.0, 22.5, 11.0 },
            { 22500, 3800, 4, 3, 1.35, 3.20, 3.7, 2.1, 23, 6.3 },
            { 23250, 4210, 3, 5, 1.45, 3.60, 3.5, 2.8, 23.5, 7.0 },
            { 20300, 3850, 2, 5, 0.90, 3.25, 3.0, 2.6, 21.5, 6.0 }
        });

        var weights = new NDArray(new double[] { 0.146, 0.144, 0.119, 0.121, 0.115, 0.101, 0.088, 0.068, 0.050, 0.048 });
        var types = new NDArray(new double[] { -1, 1, 1, 1, -1, -1, 1, 1, 1, 1 });

        var mabac = new MabacMethod(matrix, weights, types, NormalizationFunctions.MinMax);

        // Act
        var values = mabac.Resolve();
        var result = values.ToArray<double>().Select(x => Math.Round(x, 4));

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}
