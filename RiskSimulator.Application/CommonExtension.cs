namespace RiskSimulator.Application;

public static class CommonExtension 
{
    public static double ToDouble(this string source)
    {
        return double.Parse(source.Trim().TrimEnd('%')); 
    }
}