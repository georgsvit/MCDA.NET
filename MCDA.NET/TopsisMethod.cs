using NumSharp;

namespace MCDA.NET;

/// <summary>
/// Technique for Order of Preference by Similarity to Ideal Solution (TOPSIS).
/// The TOPSIS method is based on an approach in which it evaluates alternatives to a positive ideal solution and a negative ideal solution.
/// </summary>
public class TopsisMethod : McdaBaseMethod
{
    /// <summary>
    /// Function which should be used to normalize `matrix` columns.
    /// </summary>
    private readonly Func<NDArray, bool, NDArray> NormalizationFunc;

    /// <summary>
    /// TOPSIS class constructor
    /// </summary>
    /// <param name="matrix">Decision matrix / alternatives data. Alternatives are in rows and Criteria are in columns.</param>
    /// <param name="weights">Criteria weights. Sum of the weights should be 1. (e.g. sum(weights) == 1)</param>
    /// <param name="types">Array with definitions of criteria types: 1 if criteria is profit and -1 if criteria is cost for each criteria in `matrix`.</param>
    /// <param name="normalizationFunc">Function which should be used to normalize `matrix` columns. It should match signature `foo(x, cost)`, where `x` is a vector which should be normalized and `cost` is a bool variable which says if `x` is a cost or profit criterion.</param>
    public TopsisMethod(NDArray matrix, NDArray weights, NDArray types, Func<NDArray, bool, NDArray> normalizationFunc) 
        : base(matrix, weights, types)
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

        // Every row of nmatrix is multiplayed by weights
        var weightedMatrix = nMatrix * Weights;

        // Vectors of PIS and NIS
        var pis = weightedMatrix.max(0);
        var nis = weightedMatrix.min(0);

        // PIS and NIS are substracted from every row of weighted matrix
        var dp = np.sqrt(np.sum(np.power(weightedMatrix - pis, 2), 1, false, typeof(double)));
        var dm = np.sqrt(np.sum(np.power(weightedMatrix - nis, 2), 1, false, dtype: typeof(double)));

        return dm / (dm + dp);
    }
}
