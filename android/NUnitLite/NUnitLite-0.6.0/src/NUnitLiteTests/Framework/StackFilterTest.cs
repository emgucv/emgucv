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
    class StackFilterTest
    {
        private static readonly string rawTrace1 =
    @"   at NUnit.Framework.Assert.Fail(String message) in D:\Dev\NUnitLite\NUnitLite\Framework\Assert.cs:line 56" + Env.NewLine +
    @"   at NUnit.Framework.Assert.That(String label, Object actual, Matcher expectation, String message) in D:\Dev\NUnitLite\NUnitLite\Framework\Assert.cs:line 50" + Env.NewLine +
    @"   at NUnit.Framework.Assert.That(Object actual, Matcher expectation) in D:\Dev\NUnitLite\NUnitLite\Framework\Assert.cs:line 19" + Env.NewLine +
    @"   at NUnit.Tests.GreaterThanMatcherTest.MatchesGoodValue() in D:\Dev\NUnitLite\NUnitLiteTests\GreaterThanMatcherTest.cs:line 12" + Env.NewLine;

        private static readonly string filteredTrace1 =
    @"   at NUnit.Tests.GreaterThanMatcherTest.MatchesGoodValue() in D:\Dev\NUnitLite\NUnitLiteTests\GreaterThanMatcherTest.cs:line 12" + Env.NewLine;

        private static readonly string rawTrace2 =
    @"  at NUnit.Framework.Assert.Fail(String message, Object[] args)" + Env.NewLine +
    @"  at MyNamespace.MyAppsTests.AssertFailTest()" + Env.NewLine +
    @"  at System.Reflection.RuntimeMethodInfo.InternalInvoke(RuntimeMethodInfo rtmi, Object obj, BindingFlags invokeAttr, Binder binder, Object parameters, CultureInfo culture, Boolean isBinderDefault, Assembly caller, Boolean verifyAccess, StackCrawlMark& stackMark)" + Env.NewLine +
    @"  at System.Reflection.RuntimeMethodInfo.InternalInvoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture, Boolean verifyAccess, StackCrawlMark& stackMark)" + Env.NewLine +
    @"  at System.Reflection.RuntimeMethodInfo.Invoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture)" + Env.NewLine +
    @"  at System.Reflection.MethodBase.Invoke(Object obj, Object[] parameters)" + Env.NewLine +
    @"  at NUnitLite.ProxyTestCase.InvokeMethod(MethodInfo method, Object[] args)" + Env.NewLine +
    @"  at NUnit.Framework.TestCase.RunTest()" + Env.NewLine +
    @"  at NUnit.Framework.TestCase.RunBare()" + Env.NewLine +
    @"  at NUnit.Framework.TestCase.Run(TestResult result, TestListener listener)" + Env.NewLine +
    @"  at NUnit.Framework.TestCase.Run(TestListener listener)" + Env.NewLine +
    @"  at NUnit.Framework.TestSuite.Run(TestListener listener)" + Env.NewLine +
    @"  at NUnit.Framework.TestSuite.Run(TestListener listener)" + Env.NewLine +
    @"  at NUnitLite.Runner.TestRunner.Run(ITest test)" + Env.NewLine +
    @"  at NUnitLite.Runner.ConsoleUI.Run(ITest test)" + Env.NewLine +
    @"  at NUnitLite.Runner.TestRunner.Run(Assembly assembly)" + Env.NewLine +
    @"  at NUnitLite.Runner.ConsoleUI.Run()" + Env.NewLine +
    @"  at NUnitLite.Runner.ConsoleUI.Main(String[] args)" + Env.NewLine +
    @"  at OpenNETCF.Linq.Demo.Program.Main(String[] args)" + Env.NewLine;

        private static readonly string filteredTrace2 =
    @"  at MyNamespace.MyAppsTests.AssertFailTest()" + Env.NewLine +
    @"  at System.Reflection.RuntimeMethodInfo.InternalInvoke(RuntimeMethodInfo rtmi, Object obj, BindingFlags invokeAttr, Binder binder, Object parameters, CultureInfo culture, Boolean isBinderDefault, Assembly caller, Boolean verifyAccess, StackCrawlMark& stackMark)" + Env.NewLine +
    @"  at System.Reflection.RuntimeMethodInfo.InternalInvoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture, Boolean verifyAccess, StackCrawlMark& stackMark)" + Env.NewLine +
    @"  at System.Reflection.RuntimeMethodInfo.Invoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture)" + Env.NewLine +
    @"  at System.Reflection.MethodBase.Invoke(Object obj, Object[] parameters)" + Env.NewLine +
    @"  at NUnitLite.ProxyTestCase.InvokeMethod(MethodInfo method, Object[] args)" + Env.NewLine +
    @"  at NUnit.Framework.TestCase.RunTest()" + Env.NewLine +
    @"  at NUnit.Framework.TestCase.RunBare()" + Env.NewLine +
    @"  at NUnit.Framework.TestCase.Run(TestResult result, TestListener listener)" + Env.NewLine +
    @"  at NUnit.Framework.TestCase.Run(TestListener listener)" + Env.NewLine +
    @"  at NUnit.Framework.TestSuite.Run(TestListener listener)" + Env.NewLine +
    @"  at NUnit.Framework.TestSuite.Run(TestListener listener)" + Env.NewLine +
    @"  at NUnitLite.Runner.TestRunner.Run(ITest test)" + Env.NewLine +
    @"  at NUnitLite.Runner.ConsoleUI.Run(ITest test)" + Env.NewLine +
    @"  at NUnitLite.Runner.TestRunner.Run(Assembly assembly)" + Env.NewLine +
    @"  at NUnitLite.Runner.ConsoleUI.Run()" + Env.NewLine +
    @"  at NUnitLite.Runner.ConsoleUI.Main(String[] args)" + Env.NewLine +
    @"  at OpenNETCF.Linq.Demo.Program.Main(String[] args)" + Env.NewLine;

        [Test]
        public void FilterFailureTrace1()
        {
            Assert.That(StackFilter.Filter(rawTrace1), Is.EqualTo(filteredTrace1));
        }

        [Test]
        public void FilterFailureTrace2()
        {
            Assert.That(StackFilter.Filter(rawTrace2), Is.EqualTo(filteredTrace2));
        }
    }
}
