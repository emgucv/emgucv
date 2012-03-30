using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libcvblob_i386.a", LinkTarget.Simulator, ForceLoad = true, IsCxx=true)]
