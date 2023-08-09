using NumSharp;

namespace MCDA.NET;

/// <summary>
/// Multi-Attributive Border Approximation Area Comparison (MABAC) method.
/// The MABAC method is based on determining the distance measure between each possible alternative and the Boundary Approximation Area(BAA).
/// </summary>
public class MabacMethod : McdaBaseMethod
{
    /// <summary>
    /// Function which should be used to normalize `matrix` columns.
    /// </summary>
    private readonly Func<NDArray, bool, NDArray> NormalizationFunc;

    /// <summary>
    /// MABAC class constructor
    /// </summary>
    /// <param name="matrix">Decision matrix / alternatives data. Alternatives are in rows and Criteria are in columns.</param>
    /// <param name="weights">Criteria weights. Sum of the weights should be 1. (e.g. sum(weights) == 1)</param>
    /// <param name="types">Array with definitions of criteria types: 1 if criteria is profit and -1 if criteria is cost for each criteria in `matrix`.</param>
    /// <param name="normalizationFunc">Function which should be used to normalize `matrix` columns. It should match signature `foo(x, cost)`, where `x` is a vector which should be normalized and `cost` is a bool variable which says if `x` is a cost or profit criterion.</param>
    public MabacMethod(NDArray matrix, NDArray weights, NDArray types, Func<NDArray, bool, NDArray> normalizationFunc) : base(matrix, weights, types)
    {
        ValidateInputData();
        NormalizationFunc = normalizationFunc ?? NormalizationFunctions.MinMax;
    }

    /// <summary>
    /// Resolves MCD issue based on arguments passed to the constructor.
    /// </summary>
    /// <returns>NDArray of preference values for alternatives. Better alternatives have smaller values.</returns>
    public override NDArray Resolve()
    {
        var nMatrix = Helpers.NormalizeMatrix(Matrix, NormalizationFunc, Types);

        var n = nMatrix.Shape[0];

        // Calculation of the elements from the weighted matrix
        var weightedMatrix = (nMatrix + 1) * Weights;

        // Determining the border approximation area matrix
        var g = np.power(weightedMatrix.prod(axis: 0, dtype: typeof(double)), 1.0 / n);

        // Calculation of the distance border approximation area
        var q = weightedMatrix - g;

        return q.sum(1, false, typeCode: NPTypeCode.Double);
    }
}
