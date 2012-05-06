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
    class ExactTypeTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = Is.TypeOf(typeof(D1));
            GoodValues = new object[] { new D1() };
            BadValues = new object[] { new B(), new D2() };
            Description = "<NUnitLite.Tests.D1>";
        }
    }

    [TestFixture]
    class InstanceOfTypeTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = Is.InstanceOf(typeof(D1));
            GoodValues = new object[] { new D1(), new D2() };
            BadValues = new object[] { new B() };
            Description = "instance of <NUnitLite.Tests.D1>";
        }
    }

    class B { }
    class D1 : B { }
    class D2 : D1 { }
}
