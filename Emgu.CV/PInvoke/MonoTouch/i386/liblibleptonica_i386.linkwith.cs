using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("liblibleptonica_i386.a", LinkTarget.Simulator, ForceLoad = true, IsCxx = true)]
