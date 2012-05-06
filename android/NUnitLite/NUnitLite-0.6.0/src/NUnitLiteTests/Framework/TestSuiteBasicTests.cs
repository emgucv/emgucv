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
    public class TestSuiteBasicTests
    {
        [Test]
        public void CanAddTestsToSuite()
        {
            TestSuite suite = CreateSimpleSuite("my suite");
            TestSuite suite2 = CreateSimpleSuite("suite two");
            suite.AddTest(suite2);
            Assert.That( suite.TestCaseCount, Is.EqualTo( 6 ) );
        }

        [Test]
        public void CanRunTestSuite()
        {
            RecordingTestListener listener = new RecordingTestListener();
            TestSuite suite = CreateSimpleSuite("my suite");
            TestResult result = suite.Run(listener);
            Assert.That( result.ResultState, Is.EqualTo( ResultState.Success ) );
            foreach (TestResult r in result.Results)
                Assert.That( r.ResultState, Is.EqualTo( ResultState.Success ) );
            Assert.That( listener.Events, Is.EqualTo(
                "<my suite:<One::Success><Two::Success><Three::Success>:Success>" ) );
        }

        [Test]
        public void FailuresPropagateToSuite()
        {
            TestSuite suite = CreateSimpleSuite("my suite");
            DummyTestCase dummy = new DummyTestCase();
            suite.AddTest(new TestCase( "TheTest", dummy) );
            dummy.simulateTestFailure = true;
            TestResult result = suite.Run();
            Assert.That( result.ResultState, Is.EqualTo( ResultState.Failure ) );
        }

        [Test]
        public void ErrorsPropagateToSuiteAsFailures()
        {
            TestSuite suite = CreateSimpleSuite("my suite");
            DummyTestCase dummy = new DummyTestCase();
            suite.AddTest( new TestCase( "TheTest", dummy ) );
            dummy.simulateTestError = true;
            TestResult result = suite.Run();
            Assert.That( result.ResultState, Is.EqualTo( ResultState.Failure ) );
        }

        #region Helper Methods
        private TestSuite CreateSimpleSuite(string name)
        {
            TestSuite suite = new TestSuite(name);
            Assert.That( suite.TestCaseCount, Is.EqualTo( 0 ) );
            suite.AddTest( new TestCase( "One", new SimpleTestCase()));
            suite.AddTest( new TestCase( "Two", new SimpleTestCase() ) );
            suite.AddTest( new TestCase( "Three", new SimpleTestCase() ) );
            Assert.That( suite.TestCaseCount, Is.EqualTo( 3 ) );
            return suite;
        }
        #endregion
    }
}
