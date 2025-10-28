using System;
using System.Collections.Generic;

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

public static class SimpleAssert
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
            ("SumPairAddsNumbers", () => SimpleAssert.Equal(11d, Calculator.SumPair(5, 6))),
            ("SubtractRemovesSecondOperand", () => SimpleAssert.Equal(4d, Calculator.Subtract(12, 8))),
            ("MultiplyAggregatesValues", () => SimpleAssert.Equal(24d, Calculator.Multiply(new[] { 2d, 3d, 4d }))),
            ("DivideProducesFractionalResult", () => SimpleAssert.Equal(2.5d, Calculator.Divide(5, 2))),
            ("AverageMatchesManualComputation", () => SimpleAssert.Equal(3d, Calculator.Average(new[] { 1d, 3d, 5d })))
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
