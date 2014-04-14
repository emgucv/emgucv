using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith (
   "libemgucv.a", 
   LinkTarget.ArmV7s | LinkTarget.ArmV7 | LinkTarget.Simulator | LinkTarget.Arm64 | LinkTarget.Simulator64, 
   ForceLoad = true, 
   Frameworks="Foundation Accelerate CoreFoundation CoreGraphics AssetsLibrary AVFoundation CoreImage CoreMedia CoreVideo QuartzCore ImageIO", 
   LinkerFlags = "-stdlib=libstdc++ -ObjC -lc++", 
   IsCxx=true)]
