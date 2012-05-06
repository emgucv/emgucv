using System;
using NUnit.Framework;

namespace NUnitLite.Tests
{
    [TestFixture]
    public class IgnoreTests
    {
        [Test]
        public void IgnoreTestCase()
        {
            TestSuite suite = new TestSuite(typeof(FixtureWithIgnoredTestCase));
            Assert.That(suite, Is.Not.Null);
            Assert.That(suite.TestCaseCount, Is.EqualTo(1));
            TestResult result = suite.Run();
            Assert.That(result.IsSuccess);
            Assert.That(result.Results.Count, Is.EqualTo(1));
            TestResult caseResult = (TestResult)result.Results[0];
            Assert.That(caseResult.ResultState, Is.EqualTo(ResultState.NotRun));
            Assert.That(caseResult.Message, Is.EqualTo("I ignored this"));
        }

        [Test]
        public void IgnoreTestFixture()
        {
            TestSuite suite = new TestSuite(typeof(IgnoredTestFixture));
            Assert.That(suite, Is.Not.Null);
            Assert.That(suite.TestCaseCount, Is.EqualTo(2));
            Assert.That(suite.RunState, Is.EqualTo(RunState.Ignored));
            TestResult result = suite.Run();
            Assert.That(result.ResultState, Is.EqualTo( ResultState.NotRun ) );
            Assert.That(result.Message, Is.EqualTo("Ignore all the tests"));
            Assert.That(result.Results.Count, Is.EqualTo(0));
        }
    }

    class FixtureWithIgnoredTestCase
    {
        [Test,Ignore("I ignored this")]
        public void IgnoredTestCase() { }
    }

    [Ignore("Ignore all the tests")]
    class IgnoredTestFixture
    {
        [Test]
        public void SomeMethod() { }
        [Test]
        public void AnotherMethod() { }
    }
}
