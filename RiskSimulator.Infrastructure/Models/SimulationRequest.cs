using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace RiskSimulator.Infrastructure.Models;

public class SimulationRequest
{

   // [JsonIgnore]
    public string? Key { get; set; }
    public int T { get; set; }
    public int S { get; set; }
    public Asset[] assets { get; set; }
}

public class Asset
{
    public string name { get; set; }
    public double p0 { get; set; }
    public string m { get; set; }
    public string s { get; set; }
    
}
