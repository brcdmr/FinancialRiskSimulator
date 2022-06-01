namespace RiskSimulator.Application;

public static class CommonExtension 
{
    /// <summary>
    /// string extension removes special % character and parse number to double
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static double ToDouble(this string source)
    {
        return double.Parse(source.Trim().TrimEnd('%')); 
    }
}