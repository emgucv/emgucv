using System;
using NUnit.Framework;

namespace NUnitLite.Tests
{
    [TestFixture]
    public class AttributeTests
    {
        [Test]
        public void CanApplyDescriptionToTestClass()
        {
            TestSuite suite = new TestSuite(typeof(TestClassWithDescription));
            Assert.That(suite, Is.Not.Null);
            Assert.That(suite.Properties["Description"], Is.EqualTo("Class Description"));
        }

        [Test]
        public void CanApplyDescriptionToTestMethod()
        {
            TestSuite suite = new TestSuite(typeof(TestClassWithDescription));
            Assert.That(suite, Is.Not.Null);
            Assert.That(suite.TestCaseCount == 1);
            ITest test = (ITest)suite.Tests[0];
            Assert.That(test.Properties["Description"], Is.EqualTo("Method Description"));
        }

        [TestFixture, Description("Class Description")]
        class TestClassWithDescription
        {
            [Test, Description("Method Description")]
            public void TestMethodWithDescription() { }
        }

        [Test]
        public void CanApplyIgnoreAttributeToTestClass()
        {
            TestSuite suite = new TestSuite(typeof(TestClassWithIgnoreAttribute));
            Assert.That(suite, Is.Not.Null);
            Assert.That(suite.RunState, Is.EqualTo(RunState.Ignored));
            Assert.That(suite.IgnoreReason, Is.EqualTo("The Reason"));
        }

        [Test]
        public void CanApplyIgnoreAttributeToTestMethod()
        {
            TestSuite suite = new TestSuite(typeof(TestClassWithIgnoreAttribute));
            Assert.That(suite, Is.Not.Null);
            Assert.That(suite.TestCaseCount == 1);
            ITest test = (ITest)suite.Tests[0];
            Assert.That(test.RunState, Is.EqualTo(RunState.Ignored));
            Assert.That(test.IgnoreReason, Is.EqualTo("NYI"));
        }

        [Ignore("The Reason")]
        class TestClassWithIgnoreAttribute
        {
            [Test, Ignore("NYI")]
            public void TestMethodWithIgnoreAttribute() { }
        }
    }
}
