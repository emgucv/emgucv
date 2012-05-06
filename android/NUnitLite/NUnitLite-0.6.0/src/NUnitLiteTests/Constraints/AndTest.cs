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
    public class AndTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new AndConstraint(Is.GreaterThan(40), Is.LessThan(50));
            GoodValues = new object[] { 42 };
            BadValues = new object[] { 37, 53 };
            Description = "greater than 40 and less than 50";
        }

        [Test]
        public void CanCombineTestsWithAndOperator()
        {
            Assert.That(42, Is.GreaterThan(40) & Is.LessThan(50));
        }
    }
}
