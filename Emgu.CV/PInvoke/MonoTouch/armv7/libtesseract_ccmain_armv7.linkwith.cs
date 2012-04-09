using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libtesseract_ccmain_armv7.a", LinkTarget.ArmV7, ForceLoad = true, IsCxx = true)]
