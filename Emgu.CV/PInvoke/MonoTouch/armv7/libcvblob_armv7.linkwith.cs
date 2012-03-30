using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libcvblob_armv7.a", LinkTarget.ArmV7, ForceLoad = true, IsCxx=true)]
