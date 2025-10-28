using System;
using System.Collections.Generic;

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

public static class Require
{
    public static void Equal<T>(T expected, T actual, string? message = null)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            message ??= $"Expected {expected} but found {actual}.";
            throw new InvalidOperationException(message);
        }
    }
}

public static class Program
{
    public static int Main()
    {
        var tests = new (string Name, Action Test)[]
        {
            ("SumPairAddsNumbers", () => Require.Equal(10d, Calculator.SumPair(8, 2))),
            ("SubtractRemovesSecondOperand", () => Require.Equal(5d, Calculator.Subtract(14, 9))),
            ("MultiplyAggregatesValues", () => Require.Equal(20d, Calculator.Multiply(new[] { 1d, 4d, 5d }))),
            ("DivideProducesFractionalResult", () => Require.Equal(2.75d, Calculator.Divide(11, 4))),
            ("AverageMatchesManualComputation", () => Require.Equal(5d, Calculator.Average(new[] { 2d, 4d, 6d, 8d })))
        };

        var failures = new List<string>();
        foreach (var (name, test) in tests)
        {
            try
            {
                test();
                Console.WriteLine($"[PASS] {name}");
            }
            catch (Exception ex)
            {
                failures.Add($"{name}: {ex.Message}");
                Console.Error.WriteLine($"[FAIL] {name}: {ex.Message}");
            }
        }

        if (failures.Count > 0)
        {
            Console.Error.WriteLine($"{failures.Count} test(s) failed.");
            return 1;
        }

        Console.WriteLine($"All {tests.Length} tests passed.");
        return 0;
    }
}
