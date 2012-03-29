using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libopencv_contrib_i386.a", LinkTarget.Simulator, ForceLoad = true)]
