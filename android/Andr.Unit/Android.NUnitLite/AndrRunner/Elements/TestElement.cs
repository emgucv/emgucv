using System;

using NUnitLite;

namespace Android.NUnitLite.UI {
	
	abstract class TestElement : FormattedElement {
		
		string name;
		TestResult result;
		
		public TestElement (ITest test) : base (String.Empty)
		{
			if (test == null)
				throw new ArgumentNullException ("test");
		
			Test = test;
			name = test.FullName ?? test.Name;
			Caption = GetCaption ();
		}
		
		protected string Name {
			get { return name; }
		}
				
		protected TestResult Result {
			get {
				AndroidRunner.Results.TryGetValue (name, out result);
				return result;
			}
		}
		
		protected ITest Test { get; private set; }
		
		abstract protected string GetCaption ();
		
		public void Update ()
		{
			SetCaption (GetCaption ());
		}
	}
}