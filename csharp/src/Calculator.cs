using System;
using System.Collections.Generic;

namespace Regressions.CalculatorLib;

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
