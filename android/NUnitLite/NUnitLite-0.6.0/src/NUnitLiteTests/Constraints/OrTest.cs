// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace NUnitLite.Tests
{
    [TestFixture]
    public class OrTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new OrConstraint(Is.EqualTo(42), Is.EqualTo(99));
            GoodValues = new object[] { 99, 42 };
            BadValues = new object[] { 37 };
            Description = "42 or 99";
        }

        [Test]
        public void CanCombineTestsWithOrOperator()
        {
            Assert.That(99, Is.EqualTo(42) | Is.EqualTo(99) );
        }
    }
}
