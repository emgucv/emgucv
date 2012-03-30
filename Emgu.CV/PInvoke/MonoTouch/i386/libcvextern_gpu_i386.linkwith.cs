using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libcvextern_gpu_i386.a", LinkTarget.Simulator, ForceLoad = true, IsCxx=true)]
