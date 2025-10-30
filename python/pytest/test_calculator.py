import sys
from pathlib import Path

import pytest

PYTHON_ROOT = Path(__file__).resolve().parents[1]
if str(PYTHON_ROOT) not in sys.path:
    sys.path.insert(0, str(PYTHON_ROOT))

from src import average, divide, multiply, subtract, sum_pair


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
