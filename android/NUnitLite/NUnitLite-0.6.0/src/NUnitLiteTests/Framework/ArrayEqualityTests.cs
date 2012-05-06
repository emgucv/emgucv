// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using NUnit.Framework;

namespace NUnitLite.Tests
{
    [TestFixture]
    class ArrayEqualityTests
    {
        [Test]
        public void CanMatchSingleDimensionedArrays()
        {
            int[] expected = new int[] { 1, 2, 3 };
            int[] actual = new int[] { 1, 2, 3 };

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CanMatchDoubleDimensionedArrays()
        {
            int[,] expected = new int[,] { { 1, 2, 3 }, { 4, 5, 6 } };
            int[,] actual = new int[,] { { 1, 2, 3 }, { 4, 5, 6 } };

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CanMatchTripleDimensionedArrays()
        {
            int[, ,] expected = new int[,,] { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } };
            int[,,] actual = new int[,,] { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } };

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CanMatchFiveDimensionedArrays()
        {
            int[, , , ,] expected = new int[2, 2, 2, 2, 2] { { { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } }, { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } } }, { { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } }, { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } } } };
            int[, , , ,] actual = new int[2, 2, 2, 2, 2] { { { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } }, { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } } }, { { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } }, { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } } } };

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CanMatchJaggedArrays()
        {
            int[][] expected = new int[][] { new int[] { 1, 2, 3 }, new int[] { 4, 5, 6, 7 }, new int[] { 8, 9 } };
            int[][] actual = new int[][] { new int[] { 1, 2, 3 }, new int[] { 4, 5, 6, 7 }, new int[] { 8, 9 } };

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CanMatchObjectArraysWithMixedNumericTypes()
        {
            DateTime now = DateTime.Now;
            object[] expected = new object[] { 1, 2.0f, 3.5d, 7.000m, "Hello", now };
            object[] actual = new object[] { 1.0d, 2, 3.5, 7, "Hello", now };

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CanMatchArraysOfDifferentNumericTypes()
        {
            int[] expected = new int[] { 1, 2, 3 };
            double[] actual = new double[] { 1, 2, 3 };

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CanMatchArraysWithDifferentRanksAsCollection()
        {
            int[] expected = new int[] { 1, 2, 3, 4 };
            int[,] actual = new int[,] { { 1, 2 }, { 3, 4 } };

            Assert.That(actual, Is.EqualTo(expected).AsCollection);
        }

        [Test]
        public void CanMatchArraysWithDifferentDimensionsAsCollection()
        {
            int[,] expected = new int[,] { { 1, 2, 3 }, { 4, 5, 6 } };
            int[,] actual = new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };

            Assert.That(actual, Is.EqualTo(expected).AsCollection);
        }
    }
}
