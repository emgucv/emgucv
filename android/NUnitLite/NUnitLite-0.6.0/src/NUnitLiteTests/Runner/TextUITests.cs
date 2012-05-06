// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using NUnitLite.Tests;

namespace NUnitLite.Runner.Tests
{
    [TestFixture]
    public class TextUITests
    {
        [Test]
        public void CanRunTestAndReportResults()
        {
            TextWriter writer = new StringWriter();
            TextUI textUI = new TextUI(writer);
            textUI.Execute(new string[] {
                "-test:NUnitLite.Runner.Tests.SuiteReturningDummyTestSuite"});

            string report = writer.ToString();

            string fullName = typeof(DummyTestCase).FullName + ".TheTest";
            Assert.That(report, Contains.Substring("7 Tests : 1 Errors, 1 Failures, 0 Not Run"));
            Assert.That(report, Contains.Substring(string.Format("1) TheTest ({0})" + Env.NewLine + "Simulated Failure", fullName)));
            Assert.That(report, Contains.Substring(string.Format("2) TheTest ({0})" + Env.NewLine + "System.Exception : Simulated Error", fullName)));
        }
    }

    class SuiteReturningDummyTestSuite
    {
        public static ITest Suite
        {
            get { return new DummyTestSuite("SNSFSES"); }
        }
    }
}
