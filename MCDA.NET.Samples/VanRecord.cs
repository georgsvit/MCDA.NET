using CsvHelper.Configuration;

public class VanRecord
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Manufacturer { get; set; }
    public double CarryfyingCapacity { get; set; }
    public double MaxVelocity { get; set; }
    public double TravelRange { get; set; }
    public double EnginePower { get; set; }
    public double EngineTorque { get; set; }
    public double BatteryCharging100 { get; set; }
    public double BatteryCharging80 { get; set; }
    public double BatteryCapacity { get; set; }
    public double Price { get; set; }
}

public sealed class VanMap : ClassMap<VanRecord>
{
    public VanMap()
    {
        Map(m => m.Code).Name("code");
        Map(m => m.Name).Name("name");
        Map(m => m.Manufacturer).Name("manufacturer");
        Map(m => m.CarryfyingCapacity).Name("carryfying capacity");
        Map(m => m.MaxVelocity).Name("max velocity");
        Map(m => m.TravelRange).Name("travel range");
        Map(m => m.EnginePower).Name("engine power");
        Map(m => m.EngineTorque).Name("engine torque");
        Map(m => m.BatteryCharging100).Name("battery charging 100%");
        Map(m => m.BatteryCharging80).Name("battery charging 80%");
        Map(m => m.BatteryCapacity).Name("battery capacity");
        Map(m => m.Price).Name("price");
    }
}