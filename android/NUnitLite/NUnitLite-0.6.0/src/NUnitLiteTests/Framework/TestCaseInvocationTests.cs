// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.Collections;
using NUnit.Framework;

namespace NUnitLite.Tests
{
    [TestFixture]
    public class TestCaseInvocationTests
    {
        DummyTestCase dummy;
        TestCase test;
        TestResult result;

        [SetUp]
        public void SetUp()
        {
            dummy = new DummyTestCase();
            test = new TestCase("TheTest", dummy);
        }

        [Test]
        public void SetUpCalled()
        {
            RunTestAndVerifyResult(ResultState.Success);
            Assert.That(dummy.calledSetUp);
        }

        [Test]
        public void SetupFailureIsReported()
        {
            dummy.simulateSetUpFailure = true;
            RunTestAndVerifyResult(ResultState.Failure);
            Assert.That( result.Message, Is.EqualTo("Simulated SetUp Failure") );
            VerifyStackTraceContainsMethod("SetUp");
        }

        [Test]
        public void SetupErrorIsReported()
        {
            dummy.simulateSetUpError = true;
            RunTestAndVerifyResult(ResultState.Error);
            Assert.That( result.Message, Is.EqualTo( "System.Exception : Simulated SetUp Error" ) );
            VerifyStackTraceContainsMethod("SetUp");
        }

        [Test]
        public void TearDownCalled()
        {
            RunTestAndVerifyResult(ResultState.Success);
            Assert.That(dummy.calledTearDown);
        }

        [Test]
        public void TearDownCalledAfterTestFailure()
        {
            dummy.simulateTestFailure = true;
            test.Run();
            Assert.That(dummy.calledTearDown);
        }

        [Test]
        public void TearDownCalledAfterTestError()
        {
            dummy.simulateTestError = true;
            test.Run();
            Assert.That(dummy.calledTearDown);
        }

        [Test]
        public void TestAndTearDownAreNotCalledAfterSetUpFailure()
        {
            dummy.simulateSetUpFailure = true;
            test.Run();
            Assert.That( dummy.calledTheTest, Is.False, "Test" );
            Assert.That(dummy.calledTearDown, Is.False, "TearDown" );
        }

        [Test]
        public void TestAndTearDownAreNotCalledAfterSetUpError()
        {
            dummy.simulateSetUpError = true;
            test.Run();
            Assert.That(dummy.calledTheTest, Is.False, "Test");
            Assert.That(dummy.calledTearDown, Is.False, "TearDown");
        }

        [Test]
        public void TearDownFailureIsReported()
        {
            dummy.simulateTearDownFailure = true;
            RunTestAndVerifyResult(ResultState.Failure);
            Assert.That( result.Message, Is.EqualTo( "Simulated TearDown Failure" ) );
            VerifyStackTraceContainsMethod("TearDown");
        }

        //public void testTearDownFailureDoesNotOverWriteTestFailureInfo()
        //{
        //    test.simulateTestFailure = true;
        //    test.simulateTearDownFailure = true;
        //    RunTestAndVerifyResult(ResultState.Failure);
        //    NUnit.Framework.StringAssert.Contains("Simulated Test Failure", result.Message);
        //    NUnit.Framework.StringAssert.Contains("Simulated TearDown Failure", result.Message);
        //    VerifyStackTraceContainsMethod("TheTest");
        //    VerifyStackTraceContainsMethod("TearDown");
        //}

        [Test]
        public void TearDownErrorIsReported()
        {
            dummy.simulateTearDownError = true;
            RunTestAndVerifyResult(ResultState.Error);
            Assert.That( result.Message, Is.EqualTo( "System.Exception : Simulated TearDown Error" ) );
            VerifyStackTraceContainsMethod("TearDown");
        }

        [Test]
        public void TestCalled()
        {
            RunTestAndVerifyResult(ResultState.Success);
            Assert.That(dummy.calledTheTest);
        }

        [Test]
        public void TestErrorIsReported()
        {
            dummy.simulateTestError = true;
            RunTestAndVerifyResult(ResultState.Error);
            Assert.That( result.Message, Is.EqualTo( "System.Exception : Simulated Error" ) );
            VerifyStackTraceContainsMethod("TheTest");
        }

        [Test]
        public void TestFailureIsReported()
        {
            dummy.simulateTestFailure = true;
            RunTestAndVerifyResult(ResultState.Failure);
            Assert.That( result.Message, Is.EqualTo( "Simulated Failure" ) );
            VerifyStackTraceContainsMethod("TheTest");
        }

        [Test]
        public void TestListenerIsCalled()
        {
            RecordingTestListener listener = new RecordingTestListener();
            test.Run(listener);
            Assert.That( listener.Events, Is.EqualTo( "<TheTest::Success>" ) );
        }

        [Test]
        public void TestListenerReceivesFailureMessage()
        {
            RecordingTestListener listener = new RecordingTestListener();
            dummy.simulateTestFailure = true;
            test.Run(listener);
            Assert.That( listener.Events, Is.EqualTo( "<TheTest::Failure>" ) );
        }

        #region Helper Methods
        private void RunTestAndVerifyResult(ResultState expected)
        {
            result = test.Run();
            VerifyResult(expected);
        }

        private void VerifyResult(ResultState expected)
        {
            Assert.That( result.ResultState, Is.EqualTo( expected ) );
        }

        private void VerifyStackTraceContainsMethod(string methodName)
        {
#if !NETCF_1_0
            Assert.That( result.StackTrace, Is.Not.Null, "StackTrace is null" );
            string fullName = string.Format("{0}.{1}", typeof(DummyTestCase).FullName, methodName);
            Assert.That( result.StackTrace, Contains.Substring( fullName ) );
#endif
        }
        #endregion
    }
}
