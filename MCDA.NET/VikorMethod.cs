using NumSharp;

namespace MCDA.NET;

/// <summary>
/// VIšekriterijumsko KOmpromisno Rangiranje (VIKOR) method.
/// The VIKOR method is based on an approach that uses a compromise mechanism to evaluate alternatives using distance from the ideal.
/// </summary>
public class VikorMethod : McdaBaseMethod
{
    /// <summary>
    /// Weight of the strategy (see VIKOR algorithm explanation).
    /// </summary>
    private readonly double V;

    /// <summary>
    /// Function which should be used to normalize `matrix` columns.
    /// </summary>
    private readonly Func<NDArray, bool, NDArray> NormalizationFunc;

    /// <summary>
    /// VIKOR class constructor
    /// </summary>
    /// <param name="matrix">Decision matrix / alternatives data. Alternatives are in rows and Criteria are in columns.</param>
    /// <param name="weights">Criteria weights. Sum of the weights should be 1. (e.g. sum(weights) == 1)</param>
    /// <param name="types">Array with definitions of criteria types: 1 if criteria is profit and -1 if criteria is cost for each criteria in `matrix`.</param>
    /// <param name="normalizationFunc">Function which should be used to normalize `matrix` columns. It should match signature `foo(x, cost)`, where `x` is a vector which should be normalized and `cost` is a bool variable which says if `x` is a cost or profit criterion.</param>
    /// <param name="v">Weight of the strategy (see VIKOR algorithm explanation).</param>
    public VikorMethod(NDArray matrix, NDArray weights, NDArray types, Func<NDArray, bool, NDArray> normalizationFunc, double v = 0.5) 
        : base(matrix, weights, types)
    {
        ValidateInputData();
        V = v;
        NormalizationFunc = normalizationFunc ?? NormalizationFunctions.MinMax;
    }

    /// <summary>
    /// Resolves MCD issue based on arguments passed to the constructor.
    /// </summary>
    /// <returns>NDArray of preference values for alternatives. Better alternatives have smaller values.</returns>
    /// <exception cref="ArgumentException"></exception>
    public override NDArray Resolve()
    {
        var nMatrix = Helpers.NormalizeMatrix(Matrix, NormalizationFunc, Types);

        var fStar = nMatrix.max(0);
        var fminus = nMatrix.min(0);

        if (Helpers.CheckArraysContainSameElements(fStar, fminus))
        {
            var eqCriteria = np.arange(fStar.Shape[0])[fStar == fminus];

            throw new ArgumentException($"Criteria with indexes {string.Join(", ", eqCriteria)} contains equal values for all alternatives. VIKOR method could not be applied in this case. Consider removing this criteria from the decision matrix or use another MCDA method.");
        }

        var weightedFf = Weights * ((fStar - nMatrix) / (fStar - fminus));
        var s = weightedFf.sum(1, false, NPTypeCode.Double);
        var r = weightedFf.max(1, dtype: typeof(double));

        var sStar = s.min(dtype: typeof(double));
        var sMinus = s.max(dtype: typeof(double));

        var rStar = r.min(dtype: typeof(double));
        var rMinus = r.max(dtype: typeof(double));

        var q = V * (s - sStar) / (sMinus - sStar) + (1 - V) * (r - rStar) / (rMinus - rStar);

        return q;
    }
}
