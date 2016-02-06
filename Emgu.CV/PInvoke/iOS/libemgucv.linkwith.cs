//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __IOS__

using System;
using ObjCRuntime;

[assembly: LinkWith (
   "libemgucv.a", 
   LinkTarget.ArmV7s | LinkTarget.ArmV7 | LinkTarget.Simulator | LinkTarget.Arm64 | LinkTarget.Simulator64, 
   ForceLoad = true, 
   Frameworks="Foundation Accelerate CoreFoundation CoreGraphics AssetsLibrary AVFoundation CoreImage CoreMedia CoreVideo QuartzCore ImageIO", 
   LinkerFlags = "-stdlib=libc++ -ObjC -lc++", 
   IsCxx=true)]

#endif