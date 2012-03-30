using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libopencv_calib3d_i386.a", LinkTarget.Simulator, ForceLoad = true, IsCxx=true)]
