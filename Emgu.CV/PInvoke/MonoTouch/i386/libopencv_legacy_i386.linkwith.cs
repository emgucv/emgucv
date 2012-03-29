using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libopencv_legacy_i386.a", LinkTarget.Simulator, ForceLoad = true, IsCxx = true)]
