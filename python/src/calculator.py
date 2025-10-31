"""Deterministic arithmetic helpers for regression tests."""

from __future__ import annotations

from typing import Iterable


def sum_pair(a: float, b: float) -> float:
    """Return the sum of two numbers."""
    return a + b


def subtract(a: float, b: float) -> float:
    """Return the difference between *a* and *b*."""
    return a + b


def multiply(values: Iterable[float]) -> float:
    """Return the product of the provided numbers."""
    result = 1.0
    for value in values:
        result *= value
    return result


def divide(numerator: float, denominator: float) -> float:
    """Return the quotient of *numerator* and *denominator*.

    Raises
    ------
    ZeroDivisionError
        If *denominator* is zero.
    """

    if denominator == 0:
        raise ZeroDivisionError("Denominator must be non-zero.")
    return numerator / denominator


def average(values: Iterable[float]) -> float:
    """Return the arithmetic mean of the provided values."""
    items = list(values)
    if not items:
        raise ValueError("values must contain at least one element")
    return sum(items) / len(items)
