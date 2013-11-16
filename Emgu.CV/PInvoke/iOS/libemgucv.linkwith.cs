using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libemgucv.a", LinkTarget.ArmV7s | LinkTarget.ArmV7 | LinkTarget.Simulator, ForceLoad = true, Frameworks="Foundation Accelerate CoreGraphics AssetsLibrary AVFoundation CoreImage CoreMedia CoreVideo QuartzCore", LinkerFlags = "-stdlib=libc++ -ObjC -lc++", IsCxx=true)]
