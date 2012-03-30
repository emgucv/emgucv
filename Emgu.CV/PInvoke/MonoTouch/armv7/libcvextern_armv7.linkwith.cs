using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libcvextern_armv7.a", LinkTarget.ArmV7, ForceLoad = true, IsCxx=true)]
