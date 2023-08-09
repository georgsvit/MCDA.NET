using ConsoleTables;
using CsvHelper;
using MCDA.NET;
using NumSharp;
using System.Globalization;

// Read data
using var streamReader = new StreamReader("./data/vans.csv");

using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
csvReader.Context.RegisterClassMap<VanMap>();

var records = csvReader.GetRecords<VanRecord>();

// Preparing matrix
var matrix = new NDArray(records.Select(x =>
    new double[]
    {
        x.CarryfyingCapacity, x.MaxVelocity, x.TravelRange,
        x.EnginePower, x.EngineTorque, x.BatteryCharging100,
        x.BatteryCharging80, x.BatteryCapacity, x.Price
    }).ToArray());

// Preparing weights
var weights = new NDArray(Enumerable.Repeat(Math.Round(1.0 / matrix.Shape[1], 4), matrix.Shape[1]).ToArray());

// Setting types
var types = new NDArray(new[] { 1, 1, 1, 1, 1, -1, -1, 1, -1 });

var precision = 2;

// TOPSIS demonstration with four options for normalization function
var results = new Dictionary<string, Dictionary<string, object>>
{
    { nameof(NormalizationFunctions.MinMax),  new TopsisMethod(matrix, weights, types, NormalizationFunctions.MinMax).Resolve().ToArray<double>()
                                                    .Select((value, index) => ($"A{index + 1}", Math.Round(value, precision))).ToDictionary(x => x.Item1, x => (object)x.Item2) },
    { nameof(NormalizationFunctions.Max),  new TopsisMethod(matrix, weights, types, NormalizationFunctions.Max).Resolve().ToArray<double>()
                                                    .Select((value, index) => ($"A{index + 1}", Math.Round(value, precision))).ToDictionary(x => x.Item1, x => (object)x.Item2) },
    { nameof(NormalizationFunctions.Sum),  new TopsisMethod(matrix, weights, types, NormalizationFunctions.Sum).Resolve().ToArray<double>()
                                                    .Select((value, index) => ($"A{index + 1}", Math.Round(value, precision))).ToDictionary(x => x.Item1, x => (object)x.Item2) },
    { nameof(NormalizationFunctions.Vector),  new TopsisMethod(matrix, weights, types, NormalizationFunctions.Vector).Resolve().ToArray<double>()
                                                    .Select((value, index) => ($"A{index + 1}", Math.Round(value, precision))).ToDictionary(x => x.Item1, x => (object)x.Item2) }
};

Console.WriteLine("TOPSIS results");
ConsoleTable.FromDictionary(results).Configure(o => o.EnableCount = false).Write();

// VIKOR demonstration with four options for normalization function
results = new Dictionary<string, Dictionary<string, object>>
{
    { nameof(NormalizationFunctions.MinMax),  new VikorMethod(matrix, weights, types, NormalizationFunctions.MinMax).Resolve().ToArray<double>()
                                                    .Select((value, index) => ($"A{index + 1}", Math.Round(value, precision))).ToDictionary(x => x.Item1, x => (object)x.Item2) },
    { nameof(NormalizationFunctions.Max),  new VikorMethod(matrix, weights, types, NormalizationFunctions.Max).Resolve().ToArray<double>()
                                                    .Select((value, index) => ($"A{index + 1}", Math.Round(value, precision))).ToDictionary(x => x.Item1, x => (object)x.Item2) },
    { nameof(NormalizationFunctions.Sum),  new VikorMethod(matrix, weights, types, NormalizationFunctions.Sum).Resolve().ToArray<double>()
                                                    .Select((value, index) => ($"A{index + 1}", Math.Round(value, precision))).ToDictionary(x => x.Item1, x => (object)x.Item2) },
    { nameof(NormalizationFunctions.Vector),  new VikorMethod(matrix, weights, types, NormalizationFunctions.Vector).Resolve().ToArray<double>()
                                                    .Select((value, index) => ($"A{index + 1}", Math.Round(value, precision))).ToDictionary(x => x.Item1, x => (object)x.Item2) }
};

Console.WriteLine();
Console.WriteLine("VIKOR results");
ConsoleTable.FromDictionary(results).Configure(o => o.EnableCount = false).Write();

// MABAC demonstration with four options for normalization function
results = new Dictionary<string, Dictionary<string, object>>
{
    { nameof(NormalizationFunctions.MinMax),  new MabacMethod(matrix, weights, types, NormalizationFunctions.MinMax).Resolve().ToArray<double>()
                                                    .Select((value, index) => ($"A{index + 1}", Math.Round(value, precision))).ToDictionary(x => x.Item1, x => (object)x.Item2) },
    { nameof(NormalizationFunctions.Max),  new MabacMethod(matrix, weights, types, NormalizationFunctions.Max).Resolve().ToArray<double>()
                                                    .Select((value, index) => ($"A{index + 1}", Math.Round(value, precision))).ToDictionary(x => x.Item1, x => (object)x.Item2) },
    { nameof(NormalizationFunctions.Sum),  new MabacMethod(matrix, weights, types, NormalizationFunctions.Sum).Resolve().ToArray<double>()
                                                    .Select((value, index) => ($"A{index + 1}", Math.Round(value, precision))).ToDictionary(x => x.Item1, x => (object)x.Item2) },
    { nameof(NormalizationFunctions.Vector),  new MabacMethod(matrix, weights, types, NormalizationFunctions.Vector).Resolve().ToArray<double>()
                                                    .Select((value, index) => ($"A{index + 1}", Math.Round(value, precision))).ToDictionary(x => x.Item1, x => (object)x.Item2) }
};

Console.WriteLine();
Console.WriteLine("MABAC results");
ConsoleTable.FromDictionary(results).Configure(o => o.EnableCount = false).Write();

// Setting bounds additional data
var bounds = np.vstack(new NDArray(new[]
{
    np.min(matrix, axis: 0, dtype: typeof(double)).ToArray<double>(),
    np.max(matrix, axis: 0, dtype: typeof(double)).ToArray<double>()
})).T;

// SPOTIS demonstration
results = new Dictionary<string, Dictionary<string, object>>
{
    { "Preferences",  new SpotisMethod(matrix, weights, types, bounds).Resolve().ToArray<double>()
                            .Select((value, index) => ($"A{index + 1}", Math.Round(value, precision))).ToDictionary(x => x.Item1, x => (object)x.Item2) },
};

Console.WriteLine();
Console.WriteLine("SPOTIS results");
ConsoleTable.FromDictionary(results).Configure(o => o.EnableCount = false).Write();

Console.ReadLine();