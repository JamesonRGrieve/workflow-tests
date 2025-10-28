using System;
using System.Collections.Generic;
using Xunit;

namespace Regressions.XunitTests;

public static class Calculator
{
    public static double SumPair(double a, double b) => a + b;

    public static double Subtract(double a, double b) => a - b;

    public static double Multiply(IEnumerable<double> values)
    {
        double product = 1d;
        foreach (var value in values)
        {
            product *= value;
        }

        return product;
    }

    public static double Divide(double numerator, double denominator)
    {
        if (denominator == 0)
        {
            throw new DivideByZeroException("Denominator must be non-zero.");
        }

        return numerator / denominator;
    }

    public static double Average(IEnumerable<double> values)
    {
        var list = new List<double>(values);
        if (list.Count == 0)
        {
            throw new ArgumentException("values must contain at least one element", nameof(values));
        }

        double total = 0d;
        foreach (var value in list)
        {
            total += value;
        }

        return total / list.Count;
    }
}

public class CalculatorTests
{
    [Fact]
    public void SumPairAddsNumbers()
    {
        Assert.Equal(11, Calculator.SumPair(5, 6));
    }

    [Fact]
    public void SubtractRemovesSecondOperand()
    {
        Assert.Equal(4, Calculator.Subtract(12, 8));
    }

    [Fact]
    public void MultiplyAggregatesValues()
    {
        Assert.Equal(48, Calculator.Multiply(new[] { 2d, 3d, 4d }));
    }

    [Fact]
    public void DivideProducesFractionalResult()
    {
        Assert.Equal(2.5, Calculator.Divide(5, 2));
    }

    [Fact]
    public void AverageMatchesManualComputation()
    {
        Assert.Equal(3, Calculator.Average(new[] { 1d, 3d, 5d }));
    }

}
