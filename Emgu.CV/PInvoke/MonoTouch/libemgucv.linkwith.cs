using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libemgucv.a", LinkTarget.ArmV7s | LinkTarget.ArmV7 | LinkTarget.Simulator, ForceLoad = true, IsCxx=true)]
