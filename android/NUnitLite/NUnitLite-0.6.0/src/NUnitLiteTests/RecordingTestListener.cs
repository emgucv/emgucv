// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using NUnit.Framework;

namespace NUnitLite.Tests
{
    public class RecordingTestListener : TestListener
    {
        public string Events = string.Empty;

        public void TestStarted(ITest test)
        {
            Events += string.Format("<{0}:", test.Name);
        }

        public void TestFinished(TestResult result)
        {
            Events += string.Format(":{0}>", result.ResultState);
        }
    }
}
