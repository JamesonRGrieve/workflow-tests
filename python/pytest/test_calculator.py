from pathlib import Path
import sys

import pytest

ROOT = Path(__file__).resolve().parents[2]
if str(ROOT) not in sys.path:
    sys.path.insert(0, str(ROOT))

from pycalc import average, divide, multiply, subtract, sum_pair


def test_sum_pair_adds_numbers():
    assert sum_pair(3, 5) == 8


def test_subtract_removes_second_operand():
    assert subtract(10, 4) == 6


def test_multiply_multiplies_multiple_values():
    assert multiply([1, 2, 3, 4]) == 24


def test_divide_handles_fractional_results():
    assert divide(7, 2) == pytest.approx(3.5)


def test_average_matches_manual_computation():
    values = [2, 4, 6, 8]
    assert average(values) == pytest.approx(sum(values) / len(values))


def test_average_raises_on_empty_iterable():
    with pytest.raises(ValueError):
        average([])


def test_divide_by_zero_is_invalid():
    with pytest.raises(ZeroDivisionError):
        divide(1, 0)
