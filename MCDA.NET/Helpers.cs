using NumSharp;
using System.Text.RegularExpressions;

namespace MCDA.NET;

public static class Helpers
{
    /// <summary>
    /// Normalize each column in `matrix`, using `normalization` function according `criteria_types`.
    /// </summary>
    /// <param name="matrix">Decision matrix representation. The rows are considered as alternatives and the columns are considered as criteria.</param>
    /// <param name="normalizationFunc">Function which should be used for normalize `matrix` columns. It should match signature `foo(x, cost)`, where `x` is a vector which would be normalized and `cost` is a bool variable which says if `x` is a cost or profit criteria.</param>
    /// <param name="types">Describes criteria types. 1 if criteria is profit and -1 if criteria is cost for each criteria in `matrix`. If None all criteria are considered as profit</param>
    /// <returns>Normalized copy of the input matrix.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static NDArray NormalizeMatrix(NDArray matrix, Func<NDArray, bool, NDArray> normalizationFunc, NDArray types)
    {
        var nMatrix = matrix.astype(NPTypeCode.Double);
        
        if (types == null)
        {
            for (var i = 0; i < nMatrix.Shape[1]; i++)
            {
                nMatrix[Slice.All, i] = normalizationFunc(nMatrix[Slice.All, i], false);
            }
            return nMatrix;
        }

        if (matrix.Shape[1] != types!.Shape[0])
        {
            throw new ArgumentException($"Matrix has {matrix.shape[1]} criteria and criteria_types has {types.shape.Length}. This values must be equal.");
        }

        foreach (var (index, type) in types.ToArray<int>().Select((x, i) => (i, x)))
        {
            var isCost = type != 1;

            nMatrix[Slice.All, index] = normalizationFunc(nMatrix[Slice.All, index], isCost);
        }

        return nMatrix;
    }

    /// <summary>
    /// Checks if two arrays have same elements with same indexes.
    /// </summary>
    /// <param name="a">First array</param>
    /// <param name="b">Second array</param>
    /// <returns>true if arrays have same elements with same indexes, false otherwise</returns>
    /// <exception cref="ArgumentException"></exception>
    public static bool CheckArraysContainSameElements(NDArray a, NDArray b)
    {
        if (a.Shape != b.Shape)
        {
            throw new ArgumentException("Arrays must have the same shape to be processed");
        }

        for (var i = 0; i < a.Shape[0]; i++)
        {
            if (a[i].GetValue(0) == b[i].GetValue(0))
            {
                return true;
            }
        }

        return false;
    }   
}