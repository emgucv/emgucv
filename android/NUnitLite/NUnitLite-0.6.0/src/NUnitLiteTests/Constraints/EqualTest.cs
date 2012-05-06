// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Drawing;

namespace NUnitLite.Tests
{
    [TestFixture]
    public class EqualTest : ConstraintTestBase, IExpectException
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new EqualConstraint( 4 );
            GoodValues = new object[] { 4, 4.0f, 4.0d, 4.0000m };
            BadValues = new object[] { 5, null, "Hello" };
            Description = "4";
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void FailedStringMatchShowsFailurePosition()
        {
            Assert.That( "abcdgfe", Is.EqualTo( "abcdefg" ) );
        }

        static string testString = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        [Test, ExpectedException(typeof(AssertionException))]
        public void LongStringsAreTruncated()
        {
            string expected = testString;
            string actual = testString.Replace('k', 'X');

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void LongStringsAreTruncatedAtBothEndsIfNecessary()
        {
            string expected = testString;
            string actual = testString.Replace('Z', '?');

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void LongStringsAreTruncatedAtFrontEndIfNecessary()
        {
            string expected = testString;
            string actual = testString  + "+++++";

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void NANsCompareAsEqual()
        {
            Assert.That(double.NaN, Is.EqualTo(double.NaN));
        }

        //[Test]
        //public void NamedAndUnnamedColorsCompareAsEqual()
        //{
        //    EqualConstraint.SetConstraintForType(typeof(Color), typeof(SameColorAs));
        //    Assert.That(System.Drawing.Color.Red,
        //        Is.EqualTo(System.Drawing.Color.FromArgb(255, 0, 0)));
        //}

        public void HandleException(Exception ex)
        {
            StringReader rdr = new StringReader(ex.Message);
            string report = rdr.ReadLine();
            Assert.NotNull(report);
            string expected = rdr.ReadLine();
            if (expected != null && expected.Length > 11)
                expected = expected.Substring(11);
            string actual = rdr.ReadLine();
            if (actual != null && actual.Length > 11)
                actual = actual.Substring(11);
            string line = rdr.ReadLine();
            Assert.That(line, Is.Not.Null, "No caret line displayed");
            int caret = line.Substring(11).IndexOf('^');

            int minLength = Math.Min(expected.Length, actual.Length);
            int minMatch = Math.Min(caret, minLength);

            if (caret != minLength)
            {
                if (caret > minLength ||
                    expected.Substring(0, minMatch) != actual.Substring(0, minMatch) ||
                    expected[caret] == actual[caret])
                Assert.Fail("Message Error: Caret does not point at first mismatch..." + Env.NewLine + ex.Message);
            }

            if (expected.Length > 68 || actual.Length > 68 || caret > 68)
                Assert.Fail("Message Error: Strings are not truncated..." + Env.NewLine + ex.Message);
        }

        public class SameColorAs : Constraint
        {
            private Color expectedColor;

            public SameColorAs(Color expectedColor)
            {
                this.expectedColor = expectedColor;
            }

            public override bool Matches(object actual)
            {
                this.actual = actual;
                return actual is Color && ((Color)actual).ToArgb() == expectedColor.ToArgb();
            }

            public override void WriteDescriptionTo(MessageWriter writer)
            {
                writer.WriteExpectedValue( "same color as " + expectedColor.ToString());
            }
        }

    }
}
