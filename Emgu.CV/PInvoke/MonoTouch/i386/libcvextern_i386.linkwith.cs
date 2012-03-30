using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libcvextern_i386.a", LinkTarget.Simulator, ForceLoad = true, IsCxx=true)]
