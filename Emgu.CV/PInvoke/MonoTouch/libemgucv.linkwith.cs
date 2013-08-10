using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libemgucv.a", LinkTarget.ArmV7s | LinkTarget.ArmV7 | LinkTarget.Simulator, ForceLoad = true, Framework="Foundation Accelerate CoreGraphics AssetsLibrary AVFoundation CoreImage CoreMedia CoreVideo QuartzCore" IsCxx=true)]
