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
    public class NullConstraintTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new NullConstraint();
            GoodValues = new object[] { null };
            BadValues = new object[] { "hello" };
            Description = "null";
        }
    }

    [TestFixture]
    public class TrueConstraintTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new TrueConstraint();
            GoodValues = new object[] { true, 2+2==4 };
            BadValues = new object[] { null, "hello", false, 2+2==5 };
            Description = "True";
        }
    }

    [TestFixture]
    public class FalseConstraintTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new FalseConstraint();
            GoodValues = new object[] { false, 2+2==5 };
            BadValues = new object[] { null, "hello", true, 2+2==4 };
            Description = "False";
        }
    }

    [TestFixture]
    public class NaNConstraintTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new NaNConstraint();
            GoodValues = new object[] { double.NaN, float.NaN };
            BadValues = new object[] { null, "hello", 42, 4.2, 4.2f, 4.2m, 
                double.PositiveInfinity, double.NegativeInfinity,
                float.PositiveInfinity, float.NegativeInfinity };
            Description = "NaN";
        }
    }
}
