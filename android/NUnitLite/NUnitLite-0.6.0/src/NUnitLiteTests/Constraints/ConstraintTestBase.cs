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
    public abstract class ConstraintTestBase
    {
        protected Constraint Matcher;
        protected object[] GoodValues;
        protected object[] BadValues;
        protected string Description;

        [Test]
        public void SucceedsOnGoodValues()
        {
            foreach (object value in GoodValues)
                Assert.That(value, Matcher, "Test should succeed with {0}", value);
        }

        [Test]
        public void FailsOnBadValues()
        {
            foreach (object value in BadValues)
            {
                Assert.That(Matcher.Matches(value), Is.False, "Test should fail with value {0}", value);
            }
        }

        [Test]
        public void ProvidesProperDescription()
        {
            TextMessageWriter writer = new TextMessageWriter();
            Matcher.WriteDescriptionTo(writer);
            Assert.That(writer.ToString(), Is.EqualTo(Description), null);
        }

        [Test]
        public void ProvidesProperFailureMessage()
        {
            TextMessageWriter writer = new TextMessageWriter();
            Matcher.Matches(BadValues[0]);
            Matcher.WriteMessageTo(writer);
            Assert.That(writer.ToString(), Is.StringContaining(Description));
            Assert.That(writer.ToString(), Is.Not.StringContaining("<UNSET>"));
        }
    }
}
