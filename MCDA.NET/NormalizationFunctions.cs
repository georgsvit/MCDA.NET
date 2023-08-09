using NumSharp;

namespace MCDA.NET;

public static class NormalizationFunctions
{
    /// <summary>
    /// Calculates normalized vector using the min-max method.
    /// </summary>
    /// <param name="array">One-dimensional NDArray of values to be normalized.</param>
    /// <param name="isCost">Is values a vector type or profit type. Default profit type.</param>
    /// <returns>One-dimensional NDArray of normalized values.</returns>
    public static NDArray MinMax(NDArray array, bool isCost = false)
    {
        var min = array.min();
        var max = array.max();

        if (min == max)
        {
            return new NDArray(Enumerable.Repeat(array.Shape[0], 1).ToArray());
        }

        if (isCost)
        {
            return (max - array) / (max - min);
        }

        return (array - min) / (max - min);
    }

    /// <summary>
    /// Calculates normalized vector using the max method.
    /// </summary>
    /// <param name="array">One-dimensional NDArray of values to be normalized.</param>
    /// <param name="isCost">Is values a vector type or profit type. Default profit type.</param>
    /// <returns>One-dimensional NDArray of normalized values.</returns>
    public static NDArray Max(NDArray array, bool isCost = false)
    {
        var max = array.max();

        return isCost
            ? 1 - array / max
            : array / max;
    }

    /// <summary>
    /// Calculates normalized vector using the sum method.
    /// </summary>
    /// <param name="array">One-dimensional NDArray of values to be normalized.</param>
    /// <param name="isCost">Is values a vector type or profit type. Default profit type.</param>
    /// <returns>One-dimensional NDArray of normalized values.</returns>
    public static NDArray Sum(NDArray array, bool isCost = false)
    {
        return isCost
            ? (1 / array) / np.sum(1 / array, NPTypeCode.Double)
            : array / np.sum(array, NPTypeCode.Double);
    }
    
    /// <summary>
    /// Calculates normalized vector using the vector method.
    /// </summary>
    /// <param name="array">One-dimensional NDArray of values to be normalized.</param>
    /// <param name="isCost">Is values a vector type or profit type. Default profit type.</param>
    /// <returns>One-dimensional NDArray of normalized values.</returns>
    public static NDArray Vector(NDArray array, bool isCost = false)
    {
        return isCost
            ? 1 - (array / np.sqrt(np.sum(np.power(array, 2), NPTypeCode.Double)))
            : array / np.sqrt(np.sum(np.power(array, 2), NPTypeCode.Double));
    }
}
