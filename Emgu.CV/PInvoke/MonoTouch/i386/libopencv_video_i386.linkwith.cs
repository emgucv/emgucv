using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libopencv_video_i386.a", LinkTarget.Simulator, ForceLoad = true, IsCxx = true)]
