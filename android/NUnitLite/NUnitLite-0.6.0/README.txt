NUnitLite Version 0.5 - June 14, 2009

NUnitLite is a small-footprint implementation of much of the current NUnit framework. It is distributed in source form and is intended for use in situations where NUnit is too large or complex. In particular, it targets mobile and embedded environments as well as testing of applications that require "embedding" the framework in another piece of software, as when testing plugin architectures.

This file provides basic information about NUnitLite. For more info see the NUnitLite web site at http://nunitlite.com.

COPYRIGHT AND LICENSE

NUnitLite is Copyright © 2009, Charlie Poole and is licensed under the MIT license.

A copy of the license is distributed with the program in the file LICENSE.txt and is also available at http://www.opensource.org/licenses/mit-license.php.

NUNitLite is based on ideas in NUnit, but not on the NUnit implementation. In addition, some code developed in NUnitLite was subsequently contributed to the NUnit project, where it is available under the NUnit license. Subsequently, some (but not all) of the newer NUnit features were ported back to NUnitLite.

ATTRIBUTES

Classes marked with the TestFixtureAttribute represent fixtures and methods with the TestAttribute are test cases.

The SetUp and TearDown attributes are recognized as in NUnit. In methods inheriting from TestCase, the SetUp and TearDown methods may be overridden and will be called before and after each test.

A simplified form of the ExpectedExceptionAttribute allows specification of the type of the expected exception and of an ExceptionHander method, which is called to evaluate the exception in more detail.

By use of the static Suite property, arbitrary suites of tests may be manually created. A suite may consist of individual test cases, entire test fixtures or other suites.

The PropertyAttribute may be used to assign name/value pairs to any test. The DescriptionAttribute assigns descriptive text to a test. The IgnoreAttribute may be used to temporarily suppress execution of a test.

ASSERTS

The programmer expresses expected test conditions using the Assert class. The existing functionality of most current NUnit Assert methods is supported, but the syntax has been changed to use the more extensible constraint-based format. The following methods are supported:
       Assert.Null
       Assert.NotNull
       Assert.True
       Assert.False
       Assert.Fail
       Assert.That

CONSTRAINTS

NUnitLite supports most of the same built-in constraints as NUnit. Users may also derive custom constraints from the abstract Constraint class. The following built-in constraints are provided:
       AllItemsConstraint
       AndConstraint
       AssignableFromConstraint
       AssignableToConstraint
       AttributeConstraint
       AttributeExistsConstraint
       BinarySerializableConstraint (not available on compact framework)
       CollectionContainsConstraint
       CollectionEquivalentConstraint
       CollectionOrderedConstraint
       CollectionSubsetConstraint
       ContainsConstraint
       EmptyCollectionConstraint
       EmptyConstraint
       EmptyStringConstraint
       EndsWithConstraint
       EqualConstraint
       ExactTypeConstraint
       FalseConstraint
       GreaterThanConstraint
       GreaterThanOrEqualConstraint
       InstanceOfTypeConstraint
       LessThanConstraint
       LessThanOrEqualConstraint
       NaNConstraint
       NoItemConstraint
       NotConstraint
       NullConstraint
       NullOrEmptyStringConstraint
       OrConstraint
       PropertyConstraint
       PropertyExistsConstraint
       RangeConstraint
       RegexConstraint (not available on compact framework)
       SameAsConstraint
       SamePathConstraint
       SamePathOrUnderConstraint
       SomeItemsConstraint
       StartsWithConstraint
       SubstringConstraint
       ThrowsConstraint
       ThrowsNothingConstraint
       TrueConstraint
       UniqueItemsConstraint
       XmlSerializableConstraint (not available on compact framework 1.0)

Although constraints may be created using their constructors, the more usual approach is to make use of one or more of the NUnitLite SyntaxHelpers. The following helpers are provided: 

  Is: Not, All, Null, True, False, NaN, Empty, Unique, EqualTo, SameAs,
      GreaterThan, GreaterThanOrEqualTo, LessThan, LessThanOrEqualTo,
      AtLeast, AtMost, TypeOf, InstanceOf, InstanceOfType, AssignableFrom,
      AssignableTo, StringContaining, StringStarting, StringEnding, 
      StringMatching, EquivalentTo, SubsetOf, BinarySerializable, XmlSerializable, 
      Ordered, SamePath, SamePathOrUnder, InRange

  Contains: Substring, Item

  Has: No, All, Some, None,Property, Length, Count, Message, Member, Attribute

Tests are loaded as a list of fixtures, without any additional hierarchy. Each fixture contains it's tests. Tests are executed in the order found, without any guarantees of ordering. A separate instance of the fixture object is created for each test case executed by NUnitLite. The embedded console runner produces a summary of tests run and lists any errors or failures.

USAGE

NUnitLite is not "installed" in your system. Instead, you should make the NUnitLite files - those in the src/NUnitLite directory and subdirectories - part of your own project. If you wish to compile NUnitLite as a separate assembly, you may do so. Alternatively, make it part of your test project.

In either case, your tests should be created in an exe project, which is run in order to execute them. If the NUnitLite files are included in the project, the Main program will locate your tests automatically and execute them. If you place NUnitLite in a separate assembly, you will need to create a small stub that starts NUnitLite. For an example of how to do this, take a look at NUnitLite's own tests.

NUnitLite uses the NUnit.Framework namespace, which allows relatively easy portability between NUnit and NUnitLite. Test assemblies built using NUnitLite may be opened using NUnit version 2.4 or later, provided that all tests are identified using attributes rather than inheritance.

