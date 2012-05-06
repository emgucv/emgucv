// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace NUnitLite.Tests
{
    #region UniqueItemsConstraint
    [TestFixture]
    public class UniqueItemsTests : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new UniqueItemsConstraint();
            GoodValues = new object[] { new int[] { 1, 3, 17, -2, 34 }, new object[0] };
            BadValues = new object[] { new int[] { 1, 3, 17, 3, 34 } };
            Description = "all items unique";
        }
    }
    #endregion

    #region AllItemsConstraint
    [TestFixture]
    public class AllItemsTests : IExpectException
    {
        private string expectedMessage;

        [Test]
        public void AllItemsAreNotNull()
        {
            object[] c = new object[] { 1, "hello", 3, Environment.OSVersion };
            Assert.That(c, new AllItemsConstraint(Is.Not.Null));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void AllItemsAreNotNullFails()
        {
            object[] c = new object[] { 1, "hello", null, 3 };
            expectedMessage = TextMessageWriter.Pfx_Expected + "all items not null" + Env.NewLine +
                TextMessageWriter.Pfx_Actual + "< 1, \"hello\", null, 3 >" + Env.NewLine;
            Assert.That(c, new AllItemsConstraint(Is.Not.Null));
        }

        [Test]
        public void AllItemsAreInRange()
        {
            int[] c = new int[] { 12, 27, 19, 32, 45, 99, 26 };
            Assert.That(c, new AllItemsConstraint(Is.GreaterThan(10) & Is.LessThan(100)));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void AllItemsAreInRangeFailureMessage()
        {
            int[] c = new int[] { 12, 27, 19, 32, 107, 99, 26 };
            expectedMessage = 
                TextMessageWriter.Pfx_Expected + "all items greater than 10 and less than 100" + Env.NewLine +
                TextMessageWriter.Pfx_Actual   + "< 12, 27, 19, 32, 107, 99, 26 >" + Env.NewLine;
            Assert.That(c, new AllItemsConstraint(Is.GreaterThan(10) & Is.LessThan(100)));
        }

        [Test]
        public void AllItemsAreInstancesOfType()
        {
            object[] c = new object[] { 'a', 'b', 'c' };
            Assert.That(c, new AllItemsConstraint(Is.InstanceOf(typeof(char))));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void AllItemsAreInstancesOfTypeFailureMessage()
        {
            object[] c = new object[] { 'a', "b", 'c' };
            expectedMessage = 
                TextMessageWriter.Pfx_Expected + "all items instance of <System.Char>" + Env.NewLine +
                TextMessageWriter.Pfx_Actual   + "< 'a', \"b\", 'c' >" + Env.NewLine;
            Assert.That(c, new AllItemsConstraint(Is.InstanceOf(typeof(char))));
        }

        public void HandleException(Exception ex)
        {
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }
    }
    #endregion

    #region CollectionContainsConstraint
    [TestFixture]
    public class CollectionContainsTests
    {
        [Test]
        public void CanTestContentsOfArray()
        {
            object item = "xyz";
            object[] c = new object[] { 123, item, "abc" };
            Assert.That(c, new CollectionContainsConstraint(item));
        }

        [Test]
        public void CanTestContentsOfArrayList()
        {
            object item = "xyz";
            ArrayList list = new ArrayList( new object[] { 123, item, "abc" } );
            Assert.That(list, new CollectionContainsConstraint(item));
        }

#if !NETCF_1_0
        [Test]
        public void CanTestContentsOfSortedList()
        {
            object item = "xyz";
            SortedList list = new SortedList();
            list.Add("a", 123);
            list.Add("b", item);
            list.Add("c", "abc");
            Assert.That(list.Values, new CollectionContainsConstraint(item));
            Assert.That(list.Keys, new CollectionContainsConstraint("b"));
        }
#endif
    }
    #endregion

    #region CollectionEquivalentConstraint
    [TestFixture]
    public class CollectionEquivalentTests : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new CollectionEquivalentConstraint(new int[] { 1, 2, 3, 4, 5 } );
            GoodValues = new object[] { new int[] { 1, 3, 5, 4, 2 } };
            BadValues = new object[] {
                new int[] { 1, 2, 3, 7, 5 }, 
                new int[] { 1, 2, 2, 2, 5 }, 
                new int[] { 1, 2, 2, 3 , 4, 5 } };
            Description = "equivalent to < 1, 2, 3, 4, 5 >";
        }
    }
    #endregion

    #region CollectionSubsetConstraint
    [TestFixture]
    public class CollectionSubsetTests : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new CollectionSubsetConstraint(new int[] { 1, 2, 3, 4, 5 });
            GoodValues = new object[] { new int[] { 1, 3, 5 }, new int[] { 1, 2, 3, 4, 5 } };
            BadValues = new object[] { new int[] { 1, 3, 7 }, new int[] { 1, 2, 2, 2, 5 } };
            Description = "subset of < 1, 2, 3, 4, 5 >";
        }
    }
    #endregion

    #region CollectionOrderedConstraint
    [TestFixture]
    public class CollectionOrdered_Ascending : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new CollectionOrderedConstraint();
            GoodValues = new object[] { new string[] { "x", "y", "z" }, new int[] { 1, 2, 3 }, new string[] { "x", "x", "z" } };
            BadValues = new object[] { new string[] { "x", "q", "z" }, new int[] { 3, 2, 1 } };
            Description = "collection ordered";
        }
    }

    [TestFixture]
    public class CollectionOrdered_Descending : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new CollectionOrderedConstraint().Descending;
            GoodValues = new object[] { new string[] { "z", "y", "x" }, new string[] { "z", "x", "x" }, new int[] { 3, 2, 1 } };
            BadValues = new object[] { new string[] { "x", "q", "z" }, new int[] { 1, 2, 3 } };
            Description = "collection ordered, descending";
        }
    }

    [TestFixture]
    public class CollectionOrdered_Extended
    {
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void IsOrdered_Handles_null()
        {
            ArrayList al = new ArrayList();
            al.Add("x");
            al.Add(null);
            al.Add("z");

            Assert.That(al, new CollectionOrderedConstraint());
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void IsOrdered_TypesMustBeComparable()
        {
            ArrayList al = new ArrayList();
            al.Add(1);
            al.Add("x");

            Assert.That(al, new CollectionOrderedConstraint());
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void IsOrdered_AtLeastOneArgMustImplementIComparable()
        {
            ArrayList al = new ArrayList();
            al.Add(new object());
            al.Add(new object());

            Assert.That(al, new CollectionOrderedConstraint());
        }

        [Test]
        public void IsOrdered_Handles_custom_comparison()
        {
            ArrayList al = new ArrayList();
            al.Add(new object());
            al.Add(new object());

            Assert.That(al, new CollectionOrderedConstraint().Using(new AlwaysEqualComparer()));
        }

        [Test]
        public void IsOrdered_Handles_custom_comparison2()
        {
            ArrayList al = new ArrayList();
            al.Add(2);
            al.Add(1);

            Assert.That(al, new CollectionOrderedConstraint().Using(new TestComparer()));
        }

        [Test]
        public void IsOrderedBy()
        {
            ArrayList al = new ArrayList();
            al.Add(new OrderedByTestClass(1));
            al.Add(new OrderedByTestClass(2));

            Assert.That(al, new CollectionOrderedConstraint().By("Value"));
        }

        [Test]
        public void IsOrderedBy_Comparer()
        {
            ArrayList al = new ArrayList();
            al.Add(new OrderedByTestClass(1));
            al.Add(new OrderedByTestClass(2));

            Assert.That(al, new CollectionOrderedConstraint().By("Value").Using(Comparer.Default));
        }

        [Test]
        public void IsOrderedBy_Handles_heterogeneous_classes_as_long_as_the_property_is_of_same_type()
        {
            ArrayList al = new ArrayList();
            al.Add(new OrderedByTestClass(1));
            al.Add(new OrderedByTestClass2(2));

            Assert.That(al, new CollectionOrderedConstraint().By("Value"));
        }

        class OrderedByTestClass
        {
            private int myValue;

            public int Value
            {
                get { return myValue; }
                set { myValue = value; }
            }

            public OrderedByTestClass(int value)
            {
                Value = value;
            }
        }

        class OrderedByTestClass2
        {
            private int myValue;
            public int Value
            {
                get { return myValue; }
                set { myValue = value; }
            }

            public OrderedByTestClass2(int value)
            {
                Value = value;
            }
        }
    }

    class TestComparer : IComparer
    {
        #region IComparer Members

        public int Compare(object x, object y)
        {
            if (x == null && y == null)
                return 0;

            if (x == null || y == null)
                return -1;

            if (x.Equals(y))
                return 0;

            return -1;
        }

        #endregion
    }

    class AlwaysEqualComparer : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            // This comparer ALWAYS returns zero (equal)!
            return 0;
        }
    }
    #endregion
}
