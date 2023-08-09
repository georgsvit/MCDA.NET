using NumSharp;

namespace MCDA.NET;

/// <summary>
/// Stable Preference Ordering Towards Ideal Solution (SPOTIS) method.
/// The SPOTIS method is based on an approach in which it evaluates given decision alternatives using the distance from the best ideal solution.
/// </summary>
public class SpotisMethod : McdaBaseMethod
{
    /// <summary>
    /// NDArray of min and max values for each criterion. Min and max should be different values!
    /// </summary>
    private readonly NDArray Bounds;

    /// <summary>
    /// SPOTIS class constructor
    /// </summary>
    /// <param name="matrix">Decision matrix / alternatives data. Alternatives are in rows and Criteria are in columns.</param>
    /// <param name="weights">Criteria weights. Sum of the weights should be 1. (e.g. sum(weights) == 1)</param>
    /// <param name="types">Array with definitions of criteria types: 1 if criteria is profit and -1 if criteria is cost for each criteria in `matrix`.</param>
    /// <param name="bounds">Each row should contain min and max values for each criterion. Min and max should be different values.</param>
    public SpotisMethod(NDArray matrix, NDArray weights, NDArray types, NDArray bounds) : base(matrix, weights, types)
    {
        Bounds = bounds;
        ValidateInputData();
    }

    /// <summary>
    /// Validates values passed to class constructor.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public override void ValidateInputData()
    {
        base.ValidateInputData();

        if (Helpers.CheckArraysContainSameElements(Bounds[":, 0"], Bounds[":, 1"]))
        {
            var eqCriteria = np.arange(Bounds.Shape[0])[Bounds[":, 0"] == Bounds[":, 1"]];

            throw new ArgumentException($"Bounds for criteria {string.Join(", ", eqCriteria)} are equal. Consider changing min and max values for this criterion, delete this criterion or use another MCDA method.");
        }
    }

    /// <summary>
    /// Resolves MCD issue based on arguments passed to the constructor.
    /// </summary>
    /// <returns>NDArray of preference values for alternatives. Better alternatives have smaller values.</returns>
    public override NDArray Resolve()
    {
        var isp = Bounds[np.arange(Bounds.Shape[0]), ((Types + 1) / 2).astype(NPTypeCode.Int32)];

        var nMatrix = Matrix.astype(NPTypeCode.Double);

        // Normalized distances matrix (d_{ij})
        nMatrix = np.abs((nMatrix - isp) / (Bounds[":, 0"] - Bounds[":, 1"]));

        // Distances to ISP (smaller  means better alt)
        var rawScores = np.sum(nMatrix * Weights, 1, NPTypeCode.Double);

        return rawScores;
    }
}
