// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using NUnit.Framework;

namespace NUnitLite.Tests
{
    internal class SimpleTestCase
    {
        public void One() { }
        public void Two() { }
        public void Three() { }

        [Test]
        public void test1() { }
        [Test]
        public void test2() { }
        [Test]
        public void Test3() { }
        [Test]
        public void TEST4() { }

        [Test]
        internal void test5() { }
        [Test]
        public int test6() { return 0; }
        [Test]
        public void test7(int x, int y) { }  // Should not be loaded
    }

}
