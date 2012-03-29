using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libopencv_ml_i386.a", LinkTarget.Simulator, ForceLoad = true)]
