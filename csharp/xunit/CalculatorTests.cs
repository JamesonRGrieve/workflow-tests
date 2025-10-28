using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Xunit
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class FactAttribute : Attribute
    {
    }

    public static class Assert
    {
        public static void Equal<T>(T expected, T actual)
        {
            if (!EqualityComparer<T>.Default.Equals(actual, expected))
            {
                throw new InvalidOperationException($"Expected {expected} but found {actual}.");
            }
        }
    }
}

namespace Regressions.XunitTests
{
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

    public sealed class CalculatorTests
    {
        [Xunit.Fact]
        public void SumPairAddsNumbers()
        {
            Xunit.Assert.Equal(11d, Calculator.SumPair(5, 6));
        }

        [Xunit.Fact]
        public void SubtractRemovesSecondOperand()
        {
            Xunit.Assert.Equal(4d, Calculator.Subtract(12, 8));
        }

        [Xunit.Fact]
        public void MultiplyAggregatesValues()
        {
            Xunit.Assert.Equal(24d, Calculator.Multiply(new[] { 2d, 3d, 4d }));
        }

        [Xunit.Fact]
        public void DivideProducesFractionalResult()
        {
            Xunit.Assert.Equal(2.5d, Calculator.Divide(5, 2));
        }

        [Xunit.Fact]
        public void AverageMatchesManualComputation()
        {
            Xunit.Assert.Equal(3d, Calculator.Average(new[] { 1d, 3d, 5d }));
        }
    }

    public static class Program
    {
        public static int Main()
        {
            var testTypes = new[] { typeof(CalculatorTests) };
            var failures = new List<string>();

            foreach (var type in testTypes)
            {
                var instance = Activator.CreateInstance(type) ?? throw new InvalidOperationException($"Could not create instance of {type.Name}.");
                var methods = type
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(m => m.GetCustomAttribute<Xunit.FactAttribute>() != null);

                foreach (var method in methods)
                {
                    try
                    {
                        method.Invoke(instance, Array.Empty<object>());
                        Console.WriteLine($"[PASS] {type.Name}.{method.Name}");
                    }
                    catch (TargetInvocationException ex)
                    {
                        var message = ex.InnerException?.Message ?? ex.Message;
                        failures.Add($"{type.Name}.{method.Name}: {message}");
                        Console.Error.WriteLine($"[FAIL] {type.Name}.{method.Name}: {message}");
                    }
                }
            }

            if (failures.Count > 0)
            {
                Console.Error.WriteLine($"{failures.Count} test(s) failed.");
                return 1;
            }

            Console.WriteLine("All tests passed.");
            return 0;
        }
    }
}
