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
    class GreaterThanTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new GreaterThanConstraint(5);
            GoodValues = new object[] { 6 };
            BadValues = new object[] { 4, 5 };
            Description = "greater than 5";
        }
    }

    [TestFixture]
    class GreaterThanOrEqualTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new GreaterThanOrEqualConstraint(5);
            GoodValues = new object[] { 6, 5 };
            BadValues = new object[] { 4 };
            Description = "greater than or equal to 5";
        }
    }

    [TestFixture]
    class LessThanTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new LessThanConstraint(5);
            GoodValues = new object[] { 4 };
            BadValues = new object[] { 6, 5 };
            Description = "less than 5";
        }
    }

    [TestFixture]
    class LessThanOrEqualTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new LessThanOrEqualConstraint(5);
            GoodValues = new object[] { 4 , 5 };
            BadValues = new object[] { 6 };
            Description = "less than or equal to 5";
        }
    }
}
