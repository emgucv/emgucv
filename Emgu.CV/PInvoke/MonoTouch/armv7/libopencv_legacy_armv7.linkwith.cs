using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libopencv_legacy_armv7.a", LinkTarget.ArmV7, ForceLoad = true, IsCxx=true)]
