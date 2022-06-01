using Accord.Statistics;
using Accord.Statistics.Distributions.Univariate;
using RiskSimulator.Infrastructure.Models;
using RiskSimulator.Infrastructure.Services;

namespace RiskSimulator.Application;

public class MonteCarloCalculatorService : ICalculator
{
    public Task<SimulationResult> Calculate(SimulationRequest data)
    {
        SimulationResult result = new SimulationResult();

        List<double> percentiles = new List<double>() {1, 5};
        foreach (var itemAsset in data.assets)
        {
            List<double> simulatedValues = Enumerable.Repeat(itemAsset.p0, data.S).ToList();

            var tempHolder = new AssesResult() {Name = itemAsset.name};

            double mean = itemAsset.m.ToDouble();
            double stdDev = itemAsset.s.ToDouble();

            for (int steps = 1; steps < data.T; steps++)
            {
                simulatedValues = EvaluateNextValue(simulatedValues, mean, stdDev, data.S);
            }
            
            // Evaluate %1 and %5 quantiles

            foreach (var percentile in percentiles)
            {
                var tempVal = EvaluateQuantile(simulatedValues, itemAsset.p0, percentile);
                tempHolder.Quantiles.Add(new Quantiles(){Percentile = percentile, Value = tempVal});
            }
            result.FinalResults.Add(tempHolder);
        }

        return Task.FromResult(result);
    }

    private double EvaluateQuantile(List<double> simulatedValues, double firstValue,
        double percentile)
    {
        List<double> resultOverAllR = new List<double>();
        foreach (var value in simulatedValues)
        {
            double overAllR = value / firstValue - 1;
            resultOverAllR.Add(overAllR);
        }
        
        var sortedOverall = resultOverAllR.OrderBy(x => x).ToList();

        var quantile = 0.0;
        if (percentile >= 100.0d) quantile = sortedOverall[sortedOverall.Count - 1];

        double position = (double)(sortedOverall.Count + 1) * percentile / 100.0;
        double leftNumber = 0.0d, rightNumber = 0.0d;

        double n = percentile / 100.0d * (sortedOverall.Count - 1) + 1.0d;

        if (position >= 1)
        {
            leftNumber = sortedOverall[(int)System.Math.Floor(n) - 1];
            rightNumber = sortedOverall[(int)System.Math.Floor(n)];
        }
        else
        {
            leftNumber = sortedOverall[0]; 
            rightNumber = sortedOverall[1]; 
        }

        if (leftNumber == rightNumber)
            quantile= leftNumber;
        else
        {
            double part = n - System.Math.Floor(n);
            quantile = leftNumber + part * (rightNumber - leftNumber);
        }
        
        return quantile;
    }
 
    
    private List<double> EvaluateNextValue(List<double> prevValues, double mean, double stdDev, int sampleCount = 1000)
    {
        var samples = DrawRandomNumber(mean, stdDev, sampleCount);

        List<double> result = new List<double>();
        for (int i = 0; i < sampleCount; i++)
        {
            double nextValue = prevValues[i] * (samples[i] / 100 + 1);
            result.Add(nextValue);
        }
        return result;
    }
    
    private List<double> DrawRandomNumber(double mean, double stdDev, int sampleCount)
    {
        var normalDist = new NormalDistribution(mean, stdDev);
        double[] samples = normalDist.Generate(sampleCount);

        return samples.ToList();
    }
}