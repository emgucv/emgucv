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
    public class AssertionExceptionTests : IExpectException
    {
        [Test, ExpectedException(typeof(AssertionException))]
        public void CanThrowAndCatchAssertionException()
        {
            throw new AssertionException("My message");
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void CallingFailThrowsAssertionException()
        {
            Assert.Fail("My message");
        }

        public void HandleException( Exception ex )
        {
            Assert.That( ex.Message, Is.EqualTo( "My message" ) );
        }
    }
}
