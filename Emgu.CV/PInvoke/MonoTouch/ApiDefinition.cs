using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace test
{
   // Here is where you'd define your API definition for the native Objective-C library.
   //
   // For example, to bind the following Objective-C class:
   //
   //     @interface Widget : NSObject {
   //     }
   //
   // The C# binding would look like this:
   //
   //     [BaseType (typeof (NSObject))]
   //     interface Widget {
   //     }
   //
   // To bind Objective-C properties, such as:
   //
   //     @property (nonatomic, readwrite, assign) CGPoint center;
   //
   // You would add a property definition in the C# interface like so:
   //
   //     [Export ("center")]
   //     PointF Center { get; set; }
   //
   // To bind an Objective-C method, such as:
   //
   //     -(void) doSomething:(NSObject *)object atIndex:(NSInteger)index;
   //
   // You would add a method definition to the C# interface like so:
   //
   //     [Export ("doSomething:atIndex:")]
   //     void DoSomething (NSObject object, int index);
   //
   // Objective-C "constructors" such as:
   //
   //     -(id)initWithElmo:(ElmoMuppet *)elmo;
   //
   // Can be bound as:
   //
   //     [Export ("initWithElmo:")]
   //     IntPtr Constructor (ElmoMuppet elmo);
   //
   // For more information, see http://docs.xamarin.com/ios/advanced_topics/binding_objective-c_types
   //
}

