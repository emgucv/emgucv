// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.Reflection;
using NUnit.Framework;
using NUnitLite.Tests;

namespace NUnitLite.Runner.Tests
{
    [TestFixture]
    class TestLoaderTests
    {
        [Test]
        public void CanLoadSuiteFromAssembly()
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            ITest suite = TestLoader.Load(thisAssembly);
            Assert.That(suite.TestCaseCount, Is.GreaterThan(100));
            Assert.That(suite.Name, Is.EqualTo(thisAssembly.GetName().Name));

            // Don't run! It would recurse infinitely on this test.
        }

        [Test]
        public void CanLoadAndRunSuiteFromSuiteProperty()
        {
            ITest suite = TestLoader.LoadAsSuite(typeof(MyTests));
            Assert.That(suite, Is.Not.Null, "Could not get suite");
            Assert.That(suite.TestCaseCount, Is.EqualTo(4));
            RecordingTestListener listener = new RecordingTestListener();
            suite.Run(listener);
            Assert.That(listener.Events, Is.EqualTo(
                "<MyTests:<One::Success><Two::Success><TheTest::Failure><Three::Success>:Failure>"));
        }

        private class MyTests
        {
            public static ITest Suite
            {
                get
                {
                    TestSuite suite = new TestSuite("MyTests");
                    suite.AddTest( new TestCase("One", new SimpleTestCase() ) );
                    suite.AddTest( new TestCase("Two", new SimpleTestCase() ) );
                    DummyTestCase dummy = new DummyTestCase();
                    dummy.simulateTestFailure = true;
                    suite.AddTest(new TestCase( "TheTest", dummy ) );
                    suite.AddTest(new TestCase( "Three", new SimpleTestCase() ) );
                    return suite;
                }
            }
        }
    }
}
