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
    public class NotTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new NotConstraint( Is.Null );
            GoodValues = new object[] { 42, "Hello" };
            BadValues = new object [] { null };
            Description = "not null";
        }

        [Test]
        public void CanUseNotOperator()
        {
            Assert.That(42, !new EqualConstraint(99));
        }
    }
}
