// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.Collections;
using NUnit.Framework;

namespace NUnitLite.Tests
{
    [TestFixture]
    class CollectionTests : IExpectException
    {
        [Test]
        public void CanMatchTwoCollections()
        {
            ArrayList expected = new ArrayList(new int[] { 1, 2, 3 });
            ArrayList actual = new ArrayList(new int[] { 1, 2, 3 });

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CanMatchAnArrayWithACollection()
        {
            ArrayList collection = new ArrayList(new int[] { 1, 2, 3 });
            int[] array = new int[] { 1, 2, 3 };

            Assert.That(collection, Is.EqualTo(array));
            Assert.That(array, Is.EqualTo(collection));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void FailureMatchingArrayAndCollection()
        {
            int[] expected = new int[] { 1, 2, 3 };
            ArrayList actual = new ArrayList( new int[] { 1, 5, 3 } );

            Assert.That(actual, Is.EqualTo(expected));
        }

        public void HandleException(Exception ex)
        {
            Assert.That(ex.Message, Is.EqualTo(
                "  Expected is <System.Int32[3]>, actual is <System.Collections.ArrayList>" + Env.NewLine +
                "  Values differ at index [1]" + Env.NewLine +
                TextMessageWriter.Pfx_Expected + "2" + Env.NewLine +
                TextMessageWriter.Pfx_Actual   + "5" + Env.NewLine));
        }
    }
}
