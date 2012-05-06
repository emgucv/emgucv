// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.Text;
using NUnit.Framework;

namespace NUnitLite.Tests
{
    [TestFixture]
    public class ArrayFailureMessageTests : IExpectException
    {
        private static readonly string NL = Env.NewLine;

        private string expectedMessage;

        [Test, ExpectedException(typeof(AssertionException))]
        public void FailsWhenArraysHaveDifferentRanks()
        {
            int[] expected = new int[] { 1, 2, 3, 4 };
            int[,] actual = new int[,] { { 1, 2 }, { 3, 4 } };

            expectedMessage =
                "  Expected is <System.Int32[4]>, actual is <System.Int32[2,2]>" + NL;
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void FailureWhenExpectedArrayIsLonger()
        {
            int[] expected = new int[] { 1, 2, 3, 4, 5 };
            int[] actual = new int[] { 1, 2, 3 };

            expectedMessage =
                "  Expected is <System.Int32[5]>, actual is <System.Int32[3]>" + NL +
                "  Values differ at index [3]" + NL +
                "  Missing: < 4, 5 >";
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void FailureWhenActualArrayIsLonger()
        {
            int[] expected = new int[] { 1, 2, 3 };
            int[] actual = new int[] { 1, 2, 3, 4, 5, 6, 7 };

            expectedMessage =
                "  Expected is <System.Int32[3]>, actual is <System.Int32[7]>" + NL +
                "  Values differ at index [3]" + NL +
                "  Extra:   < 4, 5, 6... >";
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void FailureOnSingleDimensionedArrays()
        {
            int[] expected = new int[] { 1, 2, 3 };
            int[] actual = new int[] { 1, 5, 3 };

            expectedMessage =
                "  Expected and actual are both <System.Int32[3]>" + NL +
                "  Values differ at index [1]" + NL +
                TextMessageWriter.Pfx_Expected + "2" + NL +
                TextMessageWriter.Pfx_Actual   + "5" + NL;
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void FailureOnDoubleDimensionedArrays()
        {
            int[,] expected = new int[,] { { 1, 2, 3 }, { 4, 5, 6 } };
            int[,] actual = new int[,] { { 1, 3, 2 }, { 4, 0, 6 } };

            expectedMessage =
                "  Expected and actual are both <System.Int32[2,3]>" + NL +
                "  Values differ at index [0,1]" + NL +
                TextMessageWriter.Pfx_Expected + "2" + NL +
                TextMessageWriter.Pfx_Actual   + "3" + NL;
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void FailureOnTripleDimensionedArrays()
        {
            int[, ,] expected = new int[,,] { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } };
            int[, ,] actual = new int[,,] { { { 1, 2 }, { 3, 4 } }, { { 0, 6 }, { 7, 8 } } };

            expectedMessage =
                "  Expected and actual are both <System.Int32[2,2,2]>" + NL +
                "  Values differ at index [1,0,0]" + NL +
                TextMessageWriter.Pfx_Expected + "5" + NL +
                TextMessageWriter.Pfx_Actual   + "0" + NL;
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void FailureOnFiveDimensionedArrays()
        {
            int[, , , ,] expected = new int[2, 2, 2, 2, 2] { { { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } }, { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } } }, { { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } }, { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } } } };
            int[, , , ,] actual = new int[2, 2, 2, 2, 2] { { { { { 1, 2 }, { 4, 3 } }, { { 5, 6 }, { 7, 8 } } }, { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } } }, { { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } }, { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } } } };

            expectedMessage =
                "  Expected and actual are both <System.Int32[2,2,2,2,2]>" + NL +
                "  Values differ at index [0,0,0,1,0]" + NL +
                TextMessageWriter.Pfx_Expected + "3" + NL +
                TextMessageWriter.Pfx_Actual   + "4" + NL;
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void FailureOnJaggedArrays()
        {
            int[][] expected = new int[][] { new int[] { 1, 2, 3 }, new int[] { 4, 5, 6, 7 }, new int[] { 8, 9 } };
            int[][] actual = new int[][] { new int[] { 1, 2, 3 }, new int[] { 4, 5, 0, 7 }, new int[] { 8, 9 } };

            expectedMessage =
                "  Expected and actual are both <System.Int32[3][]>" + NL +
                "  Values differ at index [1]" + NL +
                "    Expected and actual are both <System.Int32[4]>" + NL +
                "    Values differ at index [2]" + NL +
                TextMessageWriter.Pfx_Expected + "6" + NL +
                TextMessageWriter.Pfx_Actual   + "0" + NL;
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void FailureOnJaggedArrayComparedToSimpleArray()
        {
            int[] expected = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[][] actual = new int[][] { new int[] { 1, 2, 3 }, new int[] { 4, 5, 0, 7 }, new int[] { 8, 9 } };

            expectedMessage =
                "  Expected is <System.Int32[9]>, actual is <System.Int32[3][]>" + NL +
                "  Values differ at index [0]" + NL +
                TextMessageWriter.Pfx_Expected + "1" + NL +
                TextMessageWriter.Pfx_Actual   + "< 1, 2, 3 >" + NL;
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void FailureOnArraysWithDifferentRanksAsCollection()
        {
            int[] expected = new int[] { 1, 2, 3, 4 };
            int[,] actual = new int[,] { { 1, 0 }, { 3, 4 } };

            expectedMessage =
                "  Expected is <System.Int32[4]>, actual is <System.Int32[2,2]>" + NL +
                "  Values differ at expected index [1], actual index [0,1]" + NL +
                TextMessageWriter.Pfx_Expected + "2" + NL +
                TextMessageWriter.Pfx_Actual   + "0" + NL;
            Assert.That(actual, Is.EqualTo(expected).AsCollection);
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void FailureArraysWithDifferentDimensionsAsCollection()
        {
            int[,] expected = new int[,] { { 1, 2, 3 }, { 4, 5, 6 } };
            int[,] actual = new int[,] { { 1, 2 }, { 3, 0 }, { 5, 6 } };

            expectedMessage =
                "  Expected is <System.Int32[2,3]>, actual is <System.Int32[3,2]>" + NL +
                "  Values differ at expected index [1,0], actual index [1,1]" + NL +
                TextMessageWriter.Pfx_Expected + "4" + NL +
                TextMessageWriter.Pfx_Actual   + "0" + NL;
            Assert.That(actual, Is.EqualTo(expected).AsCollection);
        }

        public void HandleException(Exception ex)
        {
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }
    }
}
