import sys
import unittest
from pathlib import Path

PYTHON_ROOT = Path(__file__).resolve().parents[1]
if str(PYTHON_ROOT) not in sys.path:
    sys.path.insert(0, str(PYTHON_ROOT))

from src import average, divide, multiply, subtract, sum_pair


class CalculatorTestCase(unittest.TestCase):
    def test_sum_pair(self) -> None:
        self.assertEqual(sum_pair(1, 9), 10)

    def test_subtract(self) -> None:
        self.assertEqual(subtract(5, 3), 2)

    def test_multiply(self) -> None:
        self.assertEqual(multiply([2, 3, 5]), 30)

    def test_divide(self) -> None:
        self.assertAlmostEqual(divide(9, 4), 2.25)

    def test_average(self) -> None:
        self.assertAlmostEqual(average([10, 20, 30]), 20.0)


if __name__ == "__main__":
    unittest.main()
