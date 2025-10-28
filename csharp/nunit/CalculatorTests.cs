using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NUnit.Framework
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TestFixtureAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TestAttribute : Attribute
    {
    }

    public static class Is
    {
        public static PredicateConstraint EqualTo<T>(T expected) =>
            new(actual => actual is T typed && EqualityComparer<T>.Default.Equals(typed, expected), $"Expected value equal to {expected}");
    }

    public readonly struct PredicateConstraint
    {
        private readonly Func<object?, bool> _predicate;
        private readonly string _message;

        public PredicateConstraint(Func<object?, bool> predicate, string message)
        {
            _predicate = predicate;
            _message = message;
        }

        public void Apply(object? actual)
        {
            if (!_predicate(actual))
            {
                throw new InvalidOperationException(_message);
            }
        }
    }

    public static class Assert
    {
        public static void That(object? actual, PredicateConstraint constraint)
        {
            constraint.Apply(actual);
        }
    }
}

namespace Regressions.NUnitTests
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

    [NUnit.Framework.TestFixture]
    public sealed class CalculatorTests
    {
        [NUnit.Framework.Test]
        public void SumPairAddsNumbers()
        {
            NUnit.Framework.Assert.That(Calculator.SumPair(8, 2), NUnit.Framework.Is.EqualTo(10d));
        }

        [NUnit.Framework.Test]
        public void SubtractRemovesSecondOperand()
        {
            NUnit.Framework.Assert.That(Calculator.Subtract(14, 9), NUnit.Framework.Is.EqualTo(5d));
        }

        [NUnit.Framework.Test]
        public void MultiplyAggregatesValues()
        {
            NUnit.Framework.Assert.That(Calculator.Multiply(new[] { 1d, 4d, 5d }), NUnit.Framework.Is.EqualTo(20d));
        }

        [NUnit.Framework.Test]
        public void DivideProducesFractionalResult()
        {
            NUnit.Framework.Assert.That(Calculator.Divide(11, 4), NUnit.Framework.Is.EqualTo(2.75d));
        }

        [NUnit.Framework.Test]
        public void AverageMatchesManualComputation()
        {
            NUnit.Framework.Assert.That(Calculator.Average(new[] { 2d, 4d, 6d, 8d }), NUnit.Framework.Is.EqualTo(5d));
        }
    }

    public static class Program
    {
        public static int Main()
        {
            var testTypes = new[] { typeof(CalculatorTests) };
            var failures = new List<string>();

            foreach (var type in testTypes.Where(t => t.GetCustomAttribute<NUnit.Framework.TestFixtureAttribute>() != null))
            {
                var instance = Activator.CreateInstance(type) ?? throw new InvalidOperationException($"Could not create instance of {type.Name}.");
                var methods = type
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(m => m.GetCustomAttribute<NUnit.Framework.TestAttribute>() != null);

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
