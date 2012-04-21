using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libemgucv.a", LinkTarget.ArmV6 | LinkTarget.ArmV7 | LinkTarget.Simulator, ForceLoad = true, IsCxx=true)]
