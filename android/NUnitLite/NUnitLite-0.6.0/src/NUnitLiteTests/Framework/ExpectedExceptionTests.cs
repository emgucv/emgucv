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
    public class ExpectedExceptionTests
    {
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSucceedsWhenSpecifiedExceptionIsThrown()
        {
            throw new ArgumentException();
        }

        [Test]
        public void TestFailsWhenNoExceptionIsThrown()
        {
            TestSuite suite = new TestSuite(typeof(NoExceptionThrownClass));
            TestResult result = (TestResult)suite.Run().Results[0];
            Assert.That(result.ResultState, Is.EqualTo(ResultState.Failure));
            Assert.That(result.Message, Is.EqualTo( 
                "Expected Exception of type <System.ArgumentException>, but none was thrown" ));
        }

        class NoExceptionThrownClass
        {
            [Test, ExpectedException(typeof(ArgumentException))]
            public void NoExceptionThrown()
            {
            }
        }

        [Test]
        public void TestFailsWhenWrongExceptionIsThrown()
        {
            TestSuite suite = new TestSuite(typeof(WrongExceptionThrownClass));
            TestResult result = (TestResult)suite.Run().Results[0];
            Assert.That(result.ResultState, Is.EqualTo(ResultState.Failure));
            Assert.That(result.Message, Is.EqualTo("Expected Exception of type <System.ArgumentException>, but was <System.ApplicationException>"));
        }

        class WrongExceptionThrownClass
        {
            [Test, ExpectedException(typeof(ArgumentException))]
            public void WrongExceptionThrown()
            {
                throw new ApplicationException();
            }
        }

        [Test]
        public void TestFailsWhenDerivedExceptionIsThrown()
        {
            TestSuite suite = new TestSuite(typeof(DerivedExceptionThrownClass));
            TestResult result = (TestResult)suite.Run().Results[0];
            Assert.That(result.ResultState, Is.EqualTo(ResultState.Failure));
            Assert.That(result.Message, Is.EqualTo("Expected Exception of type <System.Exception>, but was <System.ApplicationException>"));
        }

        class DerivedExceptionThrownClass
        {
            [Test, ExpectedException(typeof(Exception))]
            public void DerivedExceptionThrown()
            {
                throw new ApplicationException();
            }
        }

        [Test, ExpectedException]
        public void TestSucceedsWithAnyExceptionWhenNoTypeIsSpecified()
        {
            throw new AssertionException("message");
        }

        [Test]
        public void ExceptionHandlerIsCalledWhenExceptionMatches_AlternateHandler()
        {
            ExceptionHandlerCalledClass fixture = new ExceptionHandlerCalledClass();
            ITest testCase = new TestCase("ThrowsArgumentException_AlternateHandler", fixture);
            testCase.Run();
            Assert.That(fixture.HandlerCalled, Is.False, "Base Handler should not be called");
            Assert.That(fixture.AlternateHandlerCalled, "Alternate Handler should be called");
        }

        [Test]
        public void ExceptionHandlerIsCalledWhenExceptionMatches()
        {
            ExceptionHandlerCalledClass fixture = new ExceptionHandlerCalledClass();
            ITest testCase = new TestCase("ThrowsArgumentException", fixture);
            testCase.Run();
            Assert.That(fixture.HandlerCalled, Is.True, "Base Handler should be called");
            Assert.That(fixture.AlternateHandlerCalled, Is.False, "Alternate Handler should be called");
        }

        [Test]
        public void ExceptionHandlerIsNotCalledWhenExceptionDoesNotMatch()
        {
            ExceptionHandlerCalledClass fixture = new ExceptionHandlerCalledClass();
            ITest testCase = new TestCase("ThrowsApplicationException", fixture);
            testCase.Run();
            Assert.That(fixture.HandlerCalled, Is.False, "Base Handler should not be called");
            Assert.That(fixture.AlternateHandlerCalled, Is.False, "Alternate Handler should not be called");
        }

        [Test]
        public void ExceptionHandlerIsNotCalledWhenExceptionDoesNotMatch_AlternateHandler()
        {
            ExceptionHandlerCalledClass fixture = new ExceptionHandlerCalledClass();
            ITest testCase = new TestCase("ThrowsApplicationException_AlternateHandler", fixture);
            testCase.Run();
            Assert.That(fixture.HandlerCalled, Is.False, "Base Handler should not be called");
            Assert.That(fixture.AlternateHandlerCalled, Is.False, "Alternate Handler should not be called");
        }

        [Test]
        public void FailsWhenAlternateHandlerIsNotFound()
        {
            ExceptionHandlerCalledClass fixture = new ExceptionHandlerCalledClass();
            ITest testCase = new TestCase("MethodWithBadHandler", fixture);
            TestResult result = testCase.Run();
            Assert.That(result.ResultState, Is.EqualTo(ResultState.Failure));
            Assert.That(result.Message, Is.EqualTo(
                "The specified exception handler DeliberatelyMissingHandler was not found" ));
        }

        class ExceptionHandlerCalledClass : IExpectException
        {
            public bool HandlerCalled = false;
            public bool AlternateHandlerCalled = false;

            [Test, ExpectedException(typeof(ArgumentException))]
            public void ThrowsArgumentException()
            {
                throw new ArgumentException();
            }

            [Test, ExpectedException(typeof(ArgumentException), Handler = "AlternateExceptionHandler")]
            public void ThrowsArgumentException_AlternateHandler()
            {
                throw new ArgumentException();
            }

            [Test, ExpectedException(typeof(ArgumentException))]
            public void ThrowsApplicationException()
            {
                throw new ApplicationException();
            }

            [Test, ExpectedException(typeof(ArgumentException), Handler = "AlternateExceptionHandler")]
            public void ThrowsApplicationException_AlternateHandler()
            {
                throw new ApplicationException();
            }

            [Test, ExpectedException(typeof(ArgumentException), Handler = "DeliberatelyMissingHandler")]
            public void MethodWithBadHandler()
            {
                throw new ArgumentException();
            }

            public void HandleException(Exception ex)
            {
                HandlerCalled = true;
            }

            public void AlternateExceptionHandler(Exception ex)
            {
                AlternateHandlerCalled = true;
            }
        }
    }
}
