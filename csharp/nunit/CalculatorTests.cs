using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Regressions.NUnitTests;

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

[TestFixture]
public class CalculatorTests
{
    [Test]
    public void SumPairAddsNumbers()
    {
        Assert.That(Calculator.SumPair(8, 2), Is.EqualTo(10));
    }

    [Test]
    public void SubtractRemovesSecondOperand()
    {
        Assert.That(Calculator.Subtract(14, 9), Is.EqualTo(5));
    }

    [Test]
    public void MultiplyAggregatesValues()
    {
        Assert.That(Calculator.Multiply(new[] { 1d, 4d, 5d }), Is.EqualTo(20));
    }

    [Test]
    public void DivideProducesFractionalResult()
    {
        Assert.That(Calculator.Divide(11, 4), Is.EqualTo(2.75));
    }

    [Test]
    public void AverageMatchesManualComputation()
    {
        Assert.That(Calculator.Average(new[] { 2d, 4d, 6d, 8d }), Is.EqualTo(5));
    }

    [Test]
    public void DivideThrowsOnZero()
    {
        Assert.That(() => Calculator.Divide(9, 0), Throws.TypeOf<DivideByZeroException>());
    }

    [Test]
    public void AverageThrowsOnEmptySequence()
    {
        Assert.That(() => Calculator.Average(Array.Empty<double>()), Throws.ArgumentException);
    }
}
