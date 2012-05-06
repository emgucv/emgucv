// ***********************************************************************
// Copyright (c) 2007 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections;
using System.Reflection;
using NUnit.Framework;

namespace NUnitLite
{
    public class TestSuite : ITest
    {
        #region Instance Variables
        private string name;
        private string fullName;

        private RunState runState = RunState.Runnable;
        private string ignoreReason;

        private IDictionary properties = new Hashtable();

        private ArrayList tests = new ArrayList(10);
        #endregion

        #region Constructors
        public TestSuite(string name)
        {
            this.name = name;
        }

        public TestSuite(Type type)
        {
            this.name = type.Name;
            this.fullName = type.FullName;

            object[] attrs = type.GetCustomAttributes( typeof(PropertyAttribute), true);
            foreach (PropertyAttribute attr in attrs)
                foreach( DictionaryEntry entry in attr.Properties )
                    this.Properties[entry.Key] = entry.Value;

            IgnoreAttribute ignore = (IgnoreAttribute)Reflect.GetAttribute(type, typeof(IgnoreAttribute));
            if (ignore != null)
            {
                this.runState = RunState.Ignored;
                this.ignoreReason = ignore.Reason;
            }

            if ( !InvalidTestSuite(type) )
            {
                foreach (MethodInfo method in type.GetMethods())
                {
                    if (TestCase.IsTestMethod(method))
                        this.AddTest(new TestCase(method));
                    //{
                    //    ITest test = TestCase.HasValidSignature(method)
                    //        ? (ITest)new TestCase(method)
                    //        : (ITest)new InvalidTestCase(method.Name,
                    //            "Test methods must have signature void MethodName()");

                    //    this.AddTest(test);
                    //}
                }
            }
        }
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
        }

        public string FullName
        {
            get { return fullName; }
        }

        public RunState RunState
        {
            get { return runState; }
        }

        public string IgnoreReason
        {
            get { return ignoreReason; }
        }

        public IDictionary Properties
        {
            get { return properties; }
        }

        public int TestCaseCount
        {
            get
            {
                int count = 0;
                foreach (ITest test in this.tests)
                    count += test.TestCaseCount;
                return count;
            }
        }

        public IList Tests
        {
            get { return tests; }
        }
        #endregion

        #region Public Methods
        public TestResult Run()
        {
            return Run(new NullListener());
        }

        public TestResult Run(TestListener listener)
        {
            int count = 0, failures = 0, errors = 0;
            listener.TestStarted(this);
            TestResult result = new TestResult(this);

            switch (this.RunState)
            {
                case RunState.NotRunnable:
                    result.Error(this.IgnoreReason);
                    break;

                case RunState.Ignored:
                    result.NotRun(this.IgnoreReason);
                    break;

                case RunState.Runnable:
                    foreach (ITest test in tests)
                    {
                        ++count;
                        TestResult r = test.Run(listener);
                        result.AddResult(r);
                        switch (r.ResultState)
                        {
                            case ResultState.Error:
                                ++errors;
                                break;
                            case ResultState.Failure:
                                ++failures;
                                break;
                            default:
                                break;
                        }
                    }

                    if (count == 0)
                        result.NotRun("Class has no tests");
                    else if (errors > 0 || failures > 0)
                        result.Failure("One or more component tests failed");
                    else
                        result.Success();
                    break;
            }

            listener.TestFinished(result);
            return result;
        }

        public void AddTest(ITest test)
        {
            tests.Add(test);
        }
        #endregion

        #region Private Methods
        private bool InvalidTestSuite(Type type)
        {
            if (!Reflect.HasConstructor(type))
            {
                this.runState = RunState.NotRunnable;
                this.ignoreReason = string.Format( "Class {0} has no default constructor", type.Name);
                return true;
            }

            return false;
        }
        #endregion
    }
}
