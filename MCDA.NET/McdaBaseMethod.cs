using NumSharp;

namespace MCDA.NET;

public abstract class McdaBaseMethod
{
    /// <summary>
    /// Decision matrix / alternatives data.
    /// Alternatives are in rows and Criteria are in columns.
    /// </summary>
    public NDArray Matrix { get; set; }

    /// <summary>
    /// Criteria weights. Sum of the weights should be 1. (e.g. sum(weights) == 1)
    /// </summary>
    public NDArray Weights { get; set; }

    /// <summary>
    /// Array with definitions of criteria types:
    /// 1 if criteria is profit and -1 if criteria is cost for each criteria in `matrix`.
    /// </summary>
    public NDArray Types { get; set; }

    /// <summary>
    /// Base MCDA class constructor
    /// </summary>
    /// <param name="matrix">Decision matrix / alternatives data. Alternatives are in rows and Criteria are in columns.</param>
    /// <param name="weights">Criteria weights. Sum of the weights should be 1. (e.g. sum(weights) == 1)</param>
    /// <param name="types">Array with definitions of criteria types: 1 if criteria is profit and -1 if criteria is cost for each criteria in `matrix`.</param>
    protected McdaBaseMethod(NDArray matrix, NDArray weights, NDArray types)
    {
        Matrix = matrix;
        Weights = weights;
        Types = types;
    }

    public bool UseReverseRanking = true;

    /// <summary>
    /// Validates values passed to class constructor.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public virtual void ValidateInputData()
    {
        if (Matrix.Shape[1] != Weights.Shape[0] && Weights.Shape[0] != Types.Shape[0])
        {
            throw new ArgumentException("Number of criteria should be same as number of weights and number of types");
        }
    }

    /// <summary>
    /// Resolves MCD issue based on arguments passed to the constructor.
    /// </summary>
    /// <returns>NDArray of preference values for alternatives. Better alternatives have smaller values.</returns>
    public abstract NDArray Resolve();
}
