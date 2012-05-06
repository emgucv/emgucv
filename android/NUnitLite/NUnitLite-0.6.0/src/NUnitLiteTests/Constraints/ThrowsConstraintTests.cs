using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace NUnitLite.Tests
{
    [TestFixture]
    public class ThrowsConstraintTest_ExactType : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new ThrowsConstraint(
                new ExactTypeConstraint(typeof(ArgumentException)));
            Description = "<System.ArgumentException>";
            GoodValues = new object[]
            {
                new TestDelegate( TestDelegates.ThrowsArgumentException )
            };
            BadValues = new object[]
            {
                new TestDelegate( TestDelegates.ThrowsApplicationException ),
                new TestDelegate( TestDelegates.ThrowsNothing ),
                new TestDelegate( TestDelegates.ThrowsSystemException )
            };
        }
    }

    [TestFixture]
    public class ThrowsConstraintTest_InstanceOfType : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new ThrowsConstraint(
                new InstanceOfTypeConstraint(typeof(ApplicationException)));
            Description = "instance of <System.ApplicationException>";
            GoodValues = new object[]
            {
                new TestDelegate( TestDelegates.ThrowsApplicationException ),
                new TestDelegate( TestDelegates.ThrowsDerivedApplicationException )
            };

            BadValues = new object[]
            {
                new TestDelegate( TestDelegates.ThrowsArgumentException ),
                new TestDelegate( TestDelegates.ThrowsNothing ),
                new TestDelegate( TestDelegates.ThrowsSystemException )
            };
        }
    }

    [TestFixture]
    public class ThrowsConstraintTest_WithConstraint : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new ThrowsConstraint(
                new AndConstraint(
                    new ExactTypeConstraint(typeof(ArgumentException)),
                    new PropertyConstraint("Message", new SubstringConstraint("myParam"))));
            Description = @"<System.ArgumentException> and property Message String containing ""myParam""";
            GoodValues = new object[]
            {
                new TestDelegate( TestDelegates.ThrowsArgumentException )
            };
            BadValues = new object[]
            {
                new TestDelegate( TestDelegates.ThrowsApplicationException ),
                new TestDelegate( TestDelegates.ThrowsNothing ),
                new TestDelegate( TestDelegates.ThrowsSystemException )
            };
        }
    }
}
