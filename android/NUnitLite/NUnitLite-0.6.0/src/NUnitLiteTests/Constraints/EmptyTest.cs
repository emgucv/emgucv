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
    class EmptyTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new EmptyConstraint();
            GoodValues = new object[] { string.Empty, new object[0], new System.Collections.ArrayList() };
            BadValues = new object[] { "Hello", new object[] { 1, 2, 3 } };
            Description = "<empty>";
        }
    }
}
