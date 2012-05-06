// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using TMW = NUnit.Framework.TextMessageWriter;

namespace NUnitLite.Tests
{
    [TestFixture]
    class SubstringTest : ConstraintTestBase//, IExpectException
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new SubstringConstraint("hello");
            GoodValues = new object[] { "hello", "hello there", "I said hello", "say hello to fred" };
            BadValues = new object[] { "goodbye", "What the hell?", string.Empty, null };
            Description = "String containing \"hello\"";
        }

        [Test, ExpectedException]
        public void TruncatesErrorMessageCorrectly()
        {
            string a100 = new string('a', 100) + '!';
            string x500 = new string('x', 500) + '!';

            Assert.That(x500, Is.StringContaining(a100));
        }

        public void HandleException(Exception ex)
        {
            Assert.That(ex.Message, Is.EqualTo(
                //"  String did not contain expected string." + Env.NewLine +
                TextMessageWriter.Pfx_Expected + "String containing \"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa...\"" + Env.NewLine +
                TextMessageWriter.Pfx_Actual   + "\"xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx...\"" + Env.NewLine));
        }
    }

    [TestFixture]
    class SubstringTestIgnoringCase : ConstraintTestBase//, IExpectException
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new SubstringConstraint("hello").IgnoreCase;
            GoodValues = new object[] { "Hello", "HellO there", "I said HELLO", "say hello to fred" };
            BadValues = new object[] { "goodbye", "What the hell?", string.Empty, null };
            Description = "String containing \"hello\", ignoring case";
        }

        [Test, ExpectedException]
        public void TruncatesErrorMessageCorrectly()
        {
            string a100 = new string('a', 100) + '!';
            string x500 = new string('x', 500) + '!';

            Assert.That(x500, Is.StringContaining(a100).IgnoreCase);
        }

        public void HandleException(Exception ex)
        {
            Assert.That(ex.Message, Is.EqualTo(
                //"  String did not contain expected string, ignoring case." + Env.NewLine +
                TextMessageWriter.Pfx_Expected + "\"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa...\"" + Env.NewLine +
                TextMessageWriter.Pfx_Actual   + "\"xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx...\"" + Env.NewLine));
        }
    }

    [TestFixture]
    class StartsWithTest : ConstraintTestBase//, IExpectException
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new StartsWithConstraint("hello");
            GoodValues = new object[] { "hello", "hello there" };
            BadValues = new object[] { "goodbye", "What the hell?", "I said hello", "say hello to fred", string.Empty, null };
            Description = "String starting with \"hello\"";
        }

        [Test, ExpectedException]
        public void TruncatesErrorMessageCorrectly()
        {
            string a100 = new string('a', 100) + '!';
            string x500 = new string('x', 500) + '!';

            Assert.That(x500, Is.StringStarting(a100));
        }

        public void HandleException(Exception ex)
        {
            Assert.That(ex.Message, Is.EqualTo(
                //"  String did not start with expected string." + Env.NewLine +
                TextMessageWriter.Pfx_Expected + "\"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa...\"" + Env.NewLine +
                TextMessageWriter.Pfx_Actual   + "\"xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx...\"" + Env.NewLine));
        }
    }

    [TestFixture]
    class StartsWithTestIgnoringCase : ConstraintTestBase//, IExpectException
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new StartsWithConstraint("hello").IgnoreCase;
            GoodValues = new object[] { "Hello", "HELLO there" };
            BadValues = new object[] { "goodbye", "What the hell?", "I said hello", "say Hello to fred", string.Empty, null };
            Description = "String starting with \"hello\", ignoring case";
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void TruncatesErrorMessageCorrectly()
        {
            string a100 = new string('a', 100) + '!';
            string x500 = new string('x', 500) + '!';

            Assert.That(x500, Is.StringStarting(a100).IgnoreCase);
        }

        public void HandleException(Exception ex)
        {
            Assert.That(ex.Message, Is.EqualTo(
                //"  String did not start with expected string, ignoring case." + Env.NewLine +
                TextMessageWriter.Pfx_Expected + "\"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa...\"" + Env.NewLine +
                TextMessageWriter.Pfx_Actual   + "\"xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx...\"" + Env.NewLine));
        }
    }

    [TestFixture]
    public class EndsWithTest : ConstraintTestBase//, IExpectException
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new EndsWithConstraint("hello");
            GoodValues = new object[] { "hello", "I said hello" };
            BadValues = new object[] { "goodbye", "What the hell?", "hello there", "say hello to fred", string.Empty, null };
            Description = "String ending with \"hello\"";
        }

        [Test, ExpectedException]
        public void TruncatesErrorMessageCorrectly()
        {
            string a100 = new string('a', 100) + '!';
            string x500 = new string('x', 500) + '!';

            Assert.That(x500, Is.StringEnding(a100));
        }

        public void HandleException(Exception ex)
        {
            Assert.That(ex.Message, Is.EqualTo(
                //"  String did not end with expected string." + Env.NewLine +
                TextMessageWriter.Pfx_Expected + "String ending with \"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa...\"" + Env.NewLine +
                TextMessageWriter.Pfx_Actual   + "\"xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx...\"" + Env.NewLine));
        }
    }

    [TestFixture]
    public class EndsWithTestIgnoringCase : ConstraintTestBase//, IExpectException
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new EndsWithConstraint("hello").IgnoreCase;
            GoodValues = new object[] { "HELLO", "I said Hello" };
            BadValues = new object[] { "goodbye", "What the hell?", "hello there", "say hello to fred", string.Empty, null };
            Description = "String ending with \"hello\", ignoring case";
        }

        [Test, ExpectedException]
        public void TruncatesErrorMessageCorrectly()
        {
            string a100 = new string('a', 100) + '!';
            string x500 = new string('x', 500) + '!';

            Assert.That(x500, Is.StringEnding(a100).IgnoreCase);
        }

        public void HandleException(Exception ex)
        {
            Assert.That(ex.Message, Is.EqualTo(
                //"  String did not end with expected string, ignoring case." + Env.NewLine +
                TextMessageWriter.Pfx_Expected + "\"...aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa!\"" + Env.NewLine +
                TextMessageWriter.Pfx_Actual   + "\"...xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx!\"" + Env.NewLine));
        }
    }

    [TestFixture]
    class EqualIgnoringCaseTest : ConstraintTestBase, IExpectException
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new EqualConstraint("Hello World!").IgnoreCase;
            GoodValues = new object[] { "hello world!", "Hello World!", "HELLO world!" };
            BadValues = new object[] { "goodbye", "Hello Friends!", string.Empty, null };
            Description = "\"Hello World!\", ignoring case";
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void EqualIgnoringCaseProvidesCorrectErrorMessage()
        {
            Assert.That("Hello World!", new EqualConstraint("hello fred!").IgnoreCase);
        }

        public void HandleException(Exception ex)
        {
            Assert.That(ex.Message, Is.EqualTo(
                "  Expected string length 11 but was 12. Strings differ at index 6." + Env.NewLine +
                TextMessageWriter.Pfx_Expected + "\"hello fred!\", ignoring case" + Env.NewLine +
                TextMessageWriter.Pfx_Actual   + "\"Hello World!\"" + Env.NewLine +
                "  -----------------^" + Env.NewLine));
        }
    }
}
